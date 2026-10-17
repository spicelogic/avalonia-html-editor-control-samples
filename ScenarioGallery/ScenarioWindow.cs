using SpiceLogic.HtmlEditor.Abstractions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using SpiceLogic.HtmlEditor.Abstractions.DesignTime;
using SpiceLogic.HtmlEditor.Abstractions.Entities.MailMerge;
using SpiceLogic.HtmlEditor.Avalonia;
using SpiceLogic.HtmlEditor.Avalonia.Models.Dialogs;
using SpiceLogic.HtmlEditor.Avalonia.ToolbarModule;
using SpiceLogic.HtmlEditor.Resources.Localization;

namespace SpiceLogic.Samples.Avalonia;

internal sealed class ScenarioWindow : Window
{
    private readonly AvaloniaHtmlEditor _editor = new();
    private readonly SampleDocumentViewModel _model = new();
    private readonly CancellationTokenSource _lifetime = new();
    private readonly StackPanel _examples = new() { Spacing = 12, Margin = new Thickness(16) };
    private readonly TextBlock _status = new() { Text = "Starting the editor", TextWrapping = TextWrapping.Wrap };
    private readonly TextBox _export = new() { IsReadOnly = true, AcceptsReturn = true, TextWrapping = TextWrapping.Wrap };
    private readonly ContextMenu _customMenu = new();
    private bool _ready, _busy, _closed;

    internal ScenarioWindow()
    {
        Title = "SpiceLogic Avalonia HTML Editor scenarios";
        Width = 1240; Height = 920; MinWidth = 900; MinHeight = 640;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        if (Screens.Primary is { } screen)
        {
            Width = Math.Min(Width, Math.Max(MinWidth, screen.WorkingArea.Width / screen.Scaling - 32));
            Height = Math.Min(Height, Math.Max(MinHeight, screen.WorkingArea.Height / screen.Scaling - 64));
        }
        DataContext = _model;
        // Bind only one authoring property. Binding BodyHtml and DocumentHtml to
        // independently changing values would make the two inputs compete.
        _editor.Bind(AvaloniaHtmlEditor.DocumentHtmlProperty, new Binding(nameof(SampleDocumentViewModel.DocumentHtml)) { Mode = BindingMode.TwoWay });
        _editor.SpellCheckOptions.DictionaryFile.EnableUserDictionary = false;
        _editor.ErrorOccurred += (_, args) => { if (!_closed) _status.Text = args.Exception.Message; };

        AddBindingScenario(); AddCustomizationScenario(); AddDialogScenario(); AddSpellingScenario(); AddMailMergeScenario(); AddBodyDefaultsScenario(); AddAppearanceScenario();
        ConfigureOverrides();
        _examples.IsEnabled = false;

        var header = new StackPanel { Spacing = 4, Margin = new Thickness(16) };
        header.Children.Add(new TextBlock { Text = "Developer scenarios", FontSize = 22, FontWeight = FontWeight.SemiBold });
        header.Children.Add(_status);
        var root = new Grid { RowDefinitions = new RowDefinitions("Auto,*,180"), ColumnDefinitions = new ColumnDefinitions("320,*") };
        Grid.SetColumnSpan(header, 2); root.Children.Add(header);
        var sidebar = new ScrollViewer { Content = _examples, HorizontalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled };
        Grid.SetRow(sidebar, 1); root.Children.Add(sidebar);
        Grid.SetRow(_editor, 1); Grid.SetColumn(_editor, 1); root.Children.Add(_editor);
        var output = new Grid { RowDefinitions = new RowDefinitions("Auto,*"), Margin = new Thickness(16, 8) };
        output.Children.Add(new TextBlock { Text = "Awaited document export (Source changes are applied first)", Margin = new Thickness(0, 0, 0, 6) });
        Grid.SetRow(_export, 1); output.Children.Add(_export);
        Grid.SetRow(output, 2); Grid.SetColumnSpan(output, 2); root.Children.Add(output);
        Content = root;
        Opened += async (_, _) =>
        {
            try { await _editor.Ready.WaitAsync(TimeSpan.FromSeconds(30), _lifetime.Token); _ready = true; UpdateAvailability(); _status.Text = "Ready. Each section uses the actual public editor API."; }
            catch (OperationCanceledException) when (_closed) { }
            catch (Exception error) { if (!_closed) _status.Text = error.Message; }
        };
        Closed += (_, _) => { _closed = true; _lifetime.Cancel(); _editor.Dispose(); _lifetime.Dispose(); };
    }

    private void AddBindingScenario()
    {
        var panel = Section("1. Document binding and source", "DocumentHtml is a two-way MVVM binding. Source is a separate uncommitted draft until Apply, export or a mode transition.", true);
        foreach (EditorModes mode in Enum.GetValues<EditorModes>())
            Action(panel, mode switch
            {
                EditorModes.WysiwygDesign => "Design",
                EditorModes.HtmlEdit => "Source",
                EditorModes.ReadOnlyPreview => "Preview",
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            }, _ => _editor.SetEditorModeAsync(mode));
        Action(panel, "Read current document", ExportAsync);
        Action(panel, "Read XHTML", async token => _export.Text = await _editor.Content.GetDocumentHtmlAsync(true, token));
        Action(panel, "Read XML", async token => _export.Text = await _editor.Content.GetDocumentXmlAsync(token));
        Action(panel, "Embed local images", token => _editor.Content.EmbedLocalImagesAsBase64Async(token));
        Action(panel, "Prepare unsent email", PrepareEmailAsync);
        Action(panel, "Cancel source edits", _ => { _editor.CancelSourceEdits(); return Task.CompletedTask; });
        var format = new CheckBox { Content = "Indent source HTML", IsChecked = _editor.Options.FormatHtmlSource };
        format.IsCheckedChanged += async (_, _) => await RunAsync(async token =>
        { _editor.Options.FormatHtmlSource = format.IsChecked == true; await _editor.SynchronizeOptionsAsync(token); });
        panel.Children.Add(format);
        // WordWrap on the editor is document wrapping, not Source wrapping.
        // Use the actual attached AvaloniaEdit control for source presentation.
        var wrap = new CheckBox { Content = "Wrap source lines", IsChecked = _editor.GetTheRawHtmlTextEditor().WordWrap };
        wrap.IsCheckedChanged += (_, _) => _editor.GetTheRawHtmlTextEditor().WordWrap = wrap.IsChecked == true;
        panel.Children.Add(wrap);
    }

    private void AddCustomizationScenario()
    {
        var panel = Section("2. Toolbar and context menu", "The stock Save route exports to the panel below. A replacement context menu calls real awaited facades; it does not duplicate editing logic.");
        Action(panel, "Use custom context menu", _ => { _editor.EditorContextMenuStrip = _customMenu; return Task.CompletedTask; });
        Action(panel, "Restore built-in menu", _ => { _editor.EditorContextMenuStrip = null; return Task.CompletedTask; });
    }

    private void ConfigureOverrides()
    {
        _editor.ToolbarItemOverrider.OverrideAsync(ToolbarAction.Save, (_, _) => RunAsync(ExportAsync));
        var copy = new MenuItem { Header = "Copy selection" };
        copy.Click += async (_, _) => await RunAsync(token => _editor.Editor.CopyAsync(token));
        var bold = new MenuItem { Header = "Bold selection" };
        bold.Click += async (_, _) => await RunAsync(token => _editor.Formatting.BoldAsync(cancellationToken: token));
        var export = new MenuItem { Header = "Read current document" };
        export.Click += async (_, _) => await RunAsync(ExportAsync);
        _customMenu.Items.Add(copy); _customMenu.Items.Add(bold); _customMenu.Items.Add(export);
    }

    private void AddDialogScenario()
    {
        var panel = Section("3. Typed dialog factories", "A fresh custom color window uses copied inputs and guarded AcceptAsync. The same factory is invoked for nested Style/Image colors, with the actual child owner.");
        _editor.Dialog.CreateColorPickerDialogMethod = CreateColorWindow;
        _editor.Dialog.CreateStyleBuilderDialogMethod = context =>
        { Window window = context.CreateDefaultDialog(); window.Title = "Application style settings"; return window; };
        Action(panel, "Open custom color picker", token => _editor.Dialog.ShowColorPickerDialogAsync(cancellationToken: token));
        Action(panel, "Open style and nested color", token => _editor.Dialog.ShowStyleBuilderDialogAsync(token));
        Action(panel, "Open image properties", token => _editor.Dialog.ShowImageDialogAsync(token));
    }

    private Window CreateColorWindow(ColorPickerDialogContext context)
    {
        // Never cache a Window between openings. The host owns modal display,
        // DialogOpening, current-owner guards and safe focus return.
        var dialog = new Window { Title = "Application color picker", Width = 390, Height = 320, MinWidth = 320, MinHeight = 260 };
        var error = new TextBlock { TextWrapping = TextWrapping.Wrap };
        var color = new ComboBox { ItemsSource = new[] { context.InitialValue.ToString(), "#008080", "#172b4d", "#b42318", "#000000" }
            .Distinct(StringComparer.OrdinalIgnoreCase).ToArray(), SelectedIndex = 0 };
        var apply = new Button { Content = "Apply color", HorizontalAlignment = HorizontalAlignment.Stretch };
        var cancel = new Button { Content = "Cancel", HorizontalAlignment = HorizontalAlignment.Stretch };
        var content = new StackPanel { Spacing = 14, Margin = new Thickness(24) };
        content.Children.Add(new TextBlock { Text = "Choose a document color", FontSize = 20, FontWeight = FontWeight.SemiBold });
        content.Children.Add(new TextBlock { Text = "Initial value: " + context.InitialValue, TextWrapping = TextWrapping.Wrap });
        content.Children.Add(color); content.Children.Add(error); content.Children.Add(apply); content.Children.Add(cancel);
        dialog.Content = content;
        // Closing cancels a pending acceptance; it never applies a new draft.
        var lifetime = CancellationTokenSource.CreateLinkedTokenSource(_lifetime.Token);
        CancellationToken token = lifetime.Token;
        bool closed = false; dialog.Closed += (_, _) => { closed = true; lifetime.Cancel(); lifetime.Dispose(); };
        apply.Click += async (_, _) =>
        {
            if (closed || _closed) return; apply.IsEnabled = false;
            try
            {
                if (color.SelectedItem is not string value) throw new InvalidOperationException("Choose a color.");
                await context.AcceptAsync(Color.Parse(value), token);
                if (!closed) dialog.Close();
            }
            catch (Exception failure) { if (!closed) { error.Text = failure.Message; apply.IsEnabled = true; } }
        };
        cancel.Click += (_, _) => dialog.Close();
        return dialog;
    }

    private void AddSpellingScenario()
    {
        var panel = Section("4. Inline and review spelling", "Start with the bundled language dictionary, or explicitly pick one local .dic/.aff pair. This example disables user-word persistence.");
        var inline = new CheckBox { Content = "Enable inline spelling", IsChecked = _editor.EnableInlineSpellChecking };
        inline.IsCheckedChanged += async (_, _) => await RunAsync(async token =>
        { _editor.EnableInlineSpellChecking = inline.IsChecked == true; if (inline.IsChecked == true && _editor.EditorMode == EditorModes.WysiwygDesign) await _editor.ForceInlineSpellCheckAsync(token); });
        panel.Children.Add(inline);
        Action(panel, "Review spelling", token => _editor.Dialog.ShowSpellCheckerDialogAsync(token));
        Action(panel, "Choose dictionary pair", ChooseDictionaryAsync);
        Action(panel, "Use bundled dictionary", async token =>
        { RequireDesign(); _editor.SpellCheckOptions.DictionaryFile = new DictionaryFileInfo { EnableUserDictionary = false }; await _editor.ForceInlineSpellCheckAsync(token); });
    }

    private async Task ChooseDictionaryAsync(CancellationToken token)
    {
        RequireDesign();
        if (!StorageProvider.CanOpen) throw new InvalidOperationException("This host has no native file picker.");
        IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        { Title = "Choose one Hunspell .dic and matching .aff file", AllowMultiple = true,
            FileTypeFilter = new[] { new FilePickerFileType("Hunspell dictionary pair") { Patterns = new[] { "*.dic", "*.aff" } } } });
        try
        {
            token.ThrowIfCancellationRequested(); if (files.Count == 0) return;
            string[] paths = files.Select(file => file.TryGetLocalPath() ?? throw new InvalidOperationException("Choose local dictionary files.")).ToArray();
            if (paths.Length != 2 || paths.Count(path => Path.GetExtension(path).Equals(".dic", StringComparison.OrdinalIgnoreCase)) != 1 ||
                paths.Count(path => Path.GetExtension(path).Equals(".aff", StringComparison.OrdinalIgnoreCase)) != 1)
                throw new InvalidOperationException("Choose exactly one dictionary and one matching affix.");
            string dictionary = paths.Single(path => Path.GetExtension(path).Equals(".dic", StringComparison.OrdinalIgnoreCase));
            string affix = paths.Single(path => Path.GetExtension(path).Equals(".aff", StringComparison.OrdinalIgnoreCase));
            _editor.SpellCheckOptions.DictionaryFile = new DictionaryFileInfo(affix, dictionary, false, "");
            await _editor.ForceInlineSpellCheckAsync(token);
        }
        finally { foreach (IStorageFile file in files) file.Dispose(); }
    }

    private void AddMailMergeScenario()
    {
        var panel = Section("5. Mail merge", "Fields use the shared canonical placeholder markup. Merge values become literal text, including angle brackets and Unicode.");
        var name = new PlaceholderField("Customer name", "{{CustomerName}}");
        _editor.Content.MailMerge.PlaceholderFields = new List<PlaceholderField> { name };
        Action(panel, "Insert customer field", async token => { await _editor.Content.MailMerge.InsertPlaceholderAsync(name, token); });
        Action(panel, "Merge sample values", async token =>
        { await _editor.Content.MailMerge.MergeAsync(new Dictionary<string, string?> { [name.DisplayName] = "Café <North> 😀" }, token); });
        Action(panel, "Read remaining field names", async token =>
        { _export.Text = string.Join(Environment.NewLine, await _editor.Content.MailMerge.GetFieldsAsync(token)); });
    }

    private void AddBodyDefaultsScenario()
    {
        var panel = Section("6. Body defaults and reapply", "Configure Arial, 14pt and teal, then load the authored Georgia document. Loading preserves its styles. Reapply deliberately restores only the configured defaults, with real Undo.");
        Action(panel, "Configure body defaults", async token =>
        {
            await _editor.SetEditorModeAsync(EditorModes.WysiwygDesign); token.ThrowIfCancellationRequested();
            // These are authored BODY defaults, independent of native toolbar font.
            // Synchronize awaits the ordered property acknowledgments.
            _editor.DefaultFontFamily = "Arial";
            _editor.DefaultFontSizeInPt = 14;
            _editor.DefaultForeColor = Colors.Teal;
            await _editor.SynchronizeDocumentPropertiesAsync(token);
        });
        Action(panel, "Load authored Georgia document", async token =>
        {
            await _editor.SetEditorModeAsync(EditorModes.WysiwygDesign); token.ThrowIfCancellationRequested();
            await _editor.Content.SetDocumentHtmlAsync("<!DOCTYPE html><html><head><title>Authored body styles</title></head>" +
                "<body style=\"font-family:Georgia;font-size:18pt;color:#6b21a8;background-color:white;line-height:1.6\">" +
                "<h1>Authored document</h1><p>This loaded document keeps Georgia, 18pt and purple until you deliberately reapply defaults.</p></body></html>", token);
        });
        Action(panel, "Reapply configured defaults", token => { RequireDesign(); return _editor.ReapplyDefaultBodyStyleAsync(token); });
        Action(panel, "Undo last document operation", token => _editor.Editor.UndoAsync(token));
    }

    private void AddAppearanceScenario()
    {
        var panel = Section("7. Native appearance and language", "Application theme, editor interface language and toolbar icon size are separate from authored document styles. These examples preserve the current document.");
        Action(panel, "Use dark application theme", _ => { RequestedThemeVariant = ThemeVariant.Dark; return Task.CompletedTask; });
        Action(panel, "Use light application theme", _ => { RequestedThemeVariant = ThemeVariant.Light; return Task.CompletedTask; });
        Action(panel, "Use German editor interface", _ => { _editor.Language = EditorLanguage.German; return Task.CompletedTask; });
        Action(panel, "Use English editor interface", _ => { _editor.Language = EditorLanguage.EnglishUs; return Task.CompletedTask; });
        Action(panel, "Use small toolbar icons", _ => { _editor.ToolbarIconSize = ToolbarIconSizes.Small; return Task.CompletedTask; });
        Action(panel, "Use medium toolbar icons", _ => { _editor.ToolbarIconSize = ToolbarIconSizes.Medium; return Task.CompletedTask; });
        Action(panel, "Use large toolbar icons", _ => { _editor.ToolbarIconSize = ToolbarIconSizes.Large; return Task.CompletedTask; });
    }

    private void RequireDesign()
    {
        if (_editor.EditorMode != EditorModes.WysiwygDesign) throw new InvalidOperationException("Switch to Design to use this example.");
    }

    private StackPanel Section(string title, string description, bool expanded = false)
    {
        var body = new StackPanel { Spacing = 9, Margin = new Thickness(0, 8) };
        body.Children.Add(new TextBlock { Text = description, TextWrapping = TextWrapping.Wrap });
        _examples.Children.Add(new Expander { Header = title, Content = body, IsExpanded = expanded, HorizontalAlignment = HorizontalAlignment.Stretch });
        return body;
    }

    private void Action(StackPanel panel, string caption, Func<CancellationToken, Task> operation)
    {
        var button = new Button { Content = caption, HorizontalAlignment = HorizontalAlignment.Stretch };
        button.Click += async (_, _) => await RunAsync(operation); panel.Children.Add(button);
    }

    private async Task ExportAsync(CancellationToken token) => _export.Text = await _editor.Content.GetDocumentHtmlAsync(token);

    private async Task PrepareEmailAsync(CancellationToken token)
    {
        using var message = await _editor.Content.GetEmailMessageWithLocalImagesEmbeddedAsync(token);
        var html = message.AlternateViews.First(view => view.ContentType.MediaType == "text/html");
        if (html.ContentStream.CanSeek) html.ContentStream.Position = 0;
        using var reader = new StreamReader(html.ContentStream, System.Text.Encoding.UTF8, true, 1024, leaveOpen: true);
        _export.Text = await reader.ReadToEndAsync(token);
    }

    private async Task RunAsync(Func<CancellationToken, Task> operation)
    {
        if (!_ready || _busy || _closed) return;
        _busy = true; UpdateAvailability(); _status.Text = "Working";
        try { await operation(_lifetime.Token); if (!_closed) _status.Text = "Operation completed"; }
        catch (OperationCanceledException) { if (!_closed) _status.Text = "Operation canceled"; }
        catch (Exception error) { if (!_closed) _status.Text = error.Message; }
        finally { _busy = false; if (!_closed) UpdateAvailability(); }
    }

    private void UpdateAvailability() { _examples.IsEnabled = _ready && !_busy; _customMenu.IsEnabled = _ready && !_busy; }
}
