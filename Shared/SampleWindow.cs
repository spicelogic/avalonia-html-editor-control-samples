using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SpiceLogic.HtmlEditor.Abstractions;
using SpiceLogic.HtmlEditor.Avalonia;

namespace SpiceLogic.Samples;

// Shared application plumbing only. Each sample's MainWindow owns its scenario.
public class SampleWindow : Window
{
    private readonly CancellationTokenSource _lifetime = new();
    private bool _ready, _busy, _closed;
    protected AvaloniaHtmlEditor EditorControl => this.FindControl<AvaloniaHtmlEditor>("Editor")!;
    protected CancellationToken LifetimeToken { get; private set; }
    protected bool IsClosed => _closed;

    public SampleWindow()
    {
        LifetimeToken = _lifetime.Token;
        Opened += async (_, _) =>
        {
            try
            {
                // Show the native owner before waiting for its browser surface.
                await EditorControl.Ready.WaitAsync(TimeSpan.FromSeconds(30), LifetimeToken);
                EditorControl.SpellCheckOptions.DictionaryFile.EnableUserDictionary = false;
                await InitializeDocumentAsync(LifetimeToken);
                if (_closed) return;
                _ready = true;
                SetStatus("Ready. Edit the document or try a sample action.");
                SetActionsEnabled(true);
            }
            catch (OperationCanceledException) when (_closed) { }
            catch (Exception error) { if (!_closed) SetStatus(error.Message); }
        };
        Closed += (_, _) =>
        {
            _closed = true;
            _lifetime.Cancel();
            EditorControl.Dispose();
            _lifetime.Dispose();
        };
    }

    protected virtual Task InitializeDocumentAsync(CancellationToken token) =>
        EditorControl.SetDocumentHtmlAsync(SampleDocuments.ProjectBrief, token);

    protected void ConnectEditorEvents()
    {
        EditorControl.ErrorOccurred += (_, args) => { if (!_closed) SetStatus(args.Exception.Message); };
    }

    protected async Task RunAsync(Func<CancellationToken, Task> action)
    {
        if (!_ready || _busy || _closed) return;
        _busy = true;
        SetActionsEnabled(false);
        SetStatus("Working...");
        try
        {
            await action(LifetimeToken);
            if (!_closed) SetStatus("Completed. Your document is ready.");
        }
        catch (OperationCanceledException) { if (!_closed) SetStatus("Canceled."); }
        catch (Exception error) { if (!_closed) SetStatus(error.Message); }
        finally { _busy = false; if (!_closed) SetActionsEnabled(true); }
    }

    protected void ShowOutput(string text)
    {
        if (_closed) return;
        this.FindControl<TextBox>("Output")!.Text = text;
        this.FindControl<Expander>("OutputPanel")!.IsExpanded = true;
    }

    protected async Task ExportAsync(CancellationToken token) =>
        ShowOutput(await EditorControl.GetDocumentHtmlAsync(token));

    protected void RequireDesign()
    {
        if (EditorControl.EditorMode != EditorModes.WysiwygDesign)
            throw new InvalidOperationException("Switch to Design before running this action.");
    }

    protected void SetStatus(string text) => this.FindControl<TextBlock>("Status")!.Text = text;
    private void SetActionsEnabled(bool enabled) => this.FindControl<WrapPanel>("Actions")!.IsEnabled = enabled;
    protected async void Export_Click(object? sender, RoutedEventArgs e) => await RunAsync(ExportAsync);
}
