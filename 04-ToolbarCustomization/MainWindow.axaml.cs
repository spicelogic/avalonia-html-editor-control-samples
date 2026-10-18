using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpiceLogic.HtmlEditor.Abstractions;
using SpiceLogic.HtmlEditor.Avalonia.ToolbarModule;
namespace SpiceLogic.Samples;

public partial class MainWindow : SampleWindow
{

    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
        ConnectEditorEvents();
        // The stock Save button now uses our awaited export; no disk write occurs.
        EditorControl.ToolbarItemOverrider.OverrideAsync(ToolbarAction.Save, (_, _) => RunAsync(ExportAsync));
    }
    private async void Bold_Click(object? sender, RoutedEventArgs e) => await RunAsync(token =>
        EditorControl.Formatting.BoldAsync(cancellationToken: token));
    private async void Italic_Click(object? sender, RoutedEventArgs e) => await RunAsync(token =>
        EditorControl.Formatting.ItalicAsync(cancellationToken: token));
    private async void HidePrint_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ =>
    { EditorControl.ToolbarItemOverrider.ToolbarItems.Print.IsVisible = false; return Task.CompletedTask; });
    private async void RestorePrint_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ =>
    { EditorControl.ToolbarItemOverrider.ToolbarItems.Print.IsVisible = true; return Task.CompletedTask; });

}
