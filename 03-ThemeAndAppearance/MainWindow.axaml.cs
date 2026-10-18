using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpiceLogic.HtmlEditor.Abstractions;
using Avalonia.Styling;
using Avalonia.Media;
using SpiceLogic.HtmlEditor.Abstractions.DesignTime;
using SpiceLogic.HtmlEditor.Avalonia;
namespace SpiceLogic.Samples;

public partial class MainWindow : SampleWindow
{

    public MainWindow() { AvaloniaXamlLoader.Load(this); ConnectEditorEvents(); }
    private async void Dark_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ =>
    { RequestedThemeVariant = ThemeVariant.Dark; return Task.CompletedTask; });
    private async void Light_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ =>
    { RequestedThemeVariant = ThemeVariant.Light; return Task.CompletedTask; });
    private async void SmallIcons_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ =>
    { EditorControl.ToolbarIconSize = ToolbarIconSizes.Small; return Task.CompletedTask; });
    private async void LargeIcons_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ =>
    { EditorControl.ToolbarIconSize = ToolbarIconSizes.Large; return Task.CompletedTask; });
    private async void Defaults_Click(object? sender, RoutedEventArgs e) => await RunAsync(async token =>
    {
        RequireDesign();
        EditorControl.DefaultFontFamily = "Georgia";
        EditorControl.DefaultFontSizeInPt = 14;
        EditorControl.DefaultForeColor = Colors.Teal;
        await EditorControl.SynchronizeDocumentPropertiesAsync(token);
        await EditorControl.ReapplyDefaultBodyStyleAsync(token);
    });

}
