using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpiceLogic.HtmlEditor.Abstractions;

namespace SpiceLogic.Samples;

public partial class MainWindow : SampleWindow
{

    private readonly ContextMenu _menu = new();
    private readonly MenuItem _copy = new() { Header = "Copy selection" };
    private readonly MenuItem _bold = new() { Header = "Bold selection" };
    private bool _custom = true;
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this); ConnectEditorEvents();
        _copy.Click += async (_, _) => await RunAsync(token => EditorControl.Editor.CopyAsync(token));
        _bold.Click += async (_, _) => await RunAsync(token => EditorControl.Formatting.BoldAsync(cancellationToken: token));
        var export = new MenuItem { Header = "Read current HTML" };
        export.Click += async (_, _) => await RunAsync(ExportAsync);
        _menu.Items.Add(_copy); _menu.Items.Add(_bold); _menu.Items.Add(new Separator()); _menu.Items.Add(export);
        EditorControl.ContextMenuShowing += (_, args) =>
        {
            if (!_custom) return;
            // The snapshot belongs to this actual right click, not a cached selection.
            var snapshot = args.Snapshot;
            _copy.IsEnabled = snapshot?.Selection is { } selection && selection.End > selection.Start;
            _bold.IsEnabled = snapshot is { ReadOnly: false, Mode: "design" };
            args.ContextMenu = _menu;
        };
    }
    private async void Custom_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ =>
    { _custom = true; return Task.CompletedTask; });
    private async void BuiltIn_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ =>
    { _custom = false; return Task.CompletedTask; });
    private async void Preview_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ => EditorControl.SetEditorModeAsync(EditorModes.ReadOnlyPreview));
    private async void Design_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ => EditorControl.SetEditorModeAsync(EditorModes.WysiwygDesign));

}
