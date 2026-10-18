using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpiceLogic.HtmlEditor.Abstractions;
using SpiceLogic.HtmlEditor.Abstractions.Entities.MailMerge;
namespace SpiceLogic.Samples;

public partial class MainWindow : SampleWindow
{

    private readonly PlaceholderField _customer = new("Customer name", "{{CustomerName}}");
    private readonly PlaceholderField _project = new("Project name", "{{ProjectName}}");
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this); ConnectEditorEvents();
        EditorControl.Content.MailMerge.PlaceholderFields = new List<PlaceholderField> { _customer, _project };
    }
    protected override Task InitializeDocumentAsync(CancellationToken token) => EditorControl.SetDocumentHtmlAsync("""
        <!DOCTYPE html><html><head><title>Customer project update</title><style>
        body { font-family:Arial; color:#263449; padding:20px 30px; line-height:1.6; }
        h1 { color:#153c65; } .note { border-left:4px solid #008b8b; padding:12px 18px; background:#eef8f8; }
        </style></head><body><h1>Your project update</h1><p>Dear customer,</p>
        <p>Your project is ready for review. We look forward to your feedback.</p>
        <p class="note">The delivery package includes the updated content and launch checklist.</p>
        <p>Kind regards,<br>Client services</p></body></html>
        """, token);
    private async void InsertCustomer_Click(object? sender, RoutedEventArgs e) => await RunAsync(async token =>
    { RequireDesign(); await EditorControl.Content.MailMerge.InsertPlaceholderAsync(_customer, token); });
    private async void InsertProject_Click(object? sender, RoutedEventArgs e) => await RunAsync(async token =>
    { RequireDesign(); await EditorControl.Content.MailMerge.InsertPlaceholderAsync(_project, token); });
    private async void Merge_Click(object? sender, RoutedEventArgs e) => await RunAsync(async token =>
    {
        RequireDesign();
        await EditorControl.Content.MailMerge.MergeAsync(new Dictionary<string, string?>
        { [_customer.DisplayName] = "Café <North>", [_project.DisplayName] = "Autumn launch" }, token);
        await ExportAsync(token);
    });
    private async void Fields_Click(object? sender, RoutedEventArgs e) => await RunAsync(async token =>
        ShowOutput(string.Join(Environment.NewLine, await EditorControl.Content.MailMerge.GetFieldsAsync(token))));
    private async void Undo_Click(object? sender, RoutedEventArgs e) => await RunAsync(token => EditorControl.Editor.UndoAsync(token));

}
