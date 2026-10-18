using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpiceLogic.HtmlEditor.Abstractions;
using Avalonia.Data;
using SpiceLogic.HtmlEditor.Avalonia;
namespace SpiceLogic.Samples;

public partial class MainWindow : SampleWindow
{

    private readonly DocumentViewModel _model = new();
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = _model;
        // One authoring input avoids competing BodyHtml and DocumentHtml bindings.
        EditorControl.Bind(AvaloniaHtmlEditor.DocumentHtmlProperty,
            new Binding(nameof(DocumentViewModel.DocumentHtml)) { Mode = BindingMode.TwoWay });
        ConnectEditorEvents();
    }
    protected override Task InitializeDocumentAsync(CancellationToken token) => Task.CompletedTask;
    private async void LoadModel_Click(object? sender, RoutedEventArgs e) => await RunAsync(async token =>
    {
        _model.DocumentHtml = SampleDocuments.ProjectBrief.Replace("Northfield", "Lakeside", StringComparison.Ordinal);
        ShowOutput(await EditorControl.GetDocumentHtmlAsync(token));
    });
    private async void InspectModel_Click(object? sender, RoutedEventArgs e) => await RunAsync(async token =>
    {
        // Awaited export applies an outstanding Source draft before returning.
        // The returned value is the save boundary, not a potentially stale bound string.
        string current = await EditorControl.GetDocumentHtmlAsync(token);
        ShowOutput("Current export:\n" + current + "\n\nBound model:\n" + _model.DocumentHtml);
    });
    private async void Design_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ => EditorControl.SetEditorModeAsync(EditorModes.WysiwygDesign));
    private async void Source_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ => EditorControl.SetEditorModeAsync(EditorModes.HtmlEdit));
    private async void CancelSource_Click(object? sender, RoutedEventArgs e) => await RunAsync(_ =>
    { EditorControl.CancelSourceEdits(); return Task.CompletedTask; });

}
