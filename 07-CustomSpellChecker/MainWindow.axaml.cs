using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpiceLogic.HtmlEditor.Abstractions;
using SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck;
namespace SpiceLogic.Samples;

public partial class MainWindow : SampleWindow
{

    public MainWindow() { AvaloniaXamlLoader.Load(this); ConnectEditorEvents(); }
    protected override Task InitializeDocumentAsync(CancellationToken token) => EditorControl.SetDocumentHtmlAsync("""
        <!DOCTYPE html><html><head><title>Project vocabulary</title><style>
        body { font-family:Arial; color:#263449; padding:20px 30px; line-height:1.6; } h1 { color:#153c65; }
        </style></head><body><h1>Project vocabulary</h1>
        <p>The Northfield release is ready. Review the delivrey plan and the onboarding note.</p>
        <p>The custom engine accepts our project vocabulary and suggests delivery for delivrey.</p>
        </body></html>
        """, token);
    private async void Custom_Click(object? sender, RoutedEventArgs e) => await RunAsync(async token =>
    {
        RequireDesign();
        EditorControl.SpellCheckOptions.CustomSpellCheckerEngine = new ProjectSpellChecker();
        EditorControl.SpellCheckOptions.SpellChecker = SpellCheckerEngineTypes.Custom;
        await EditorControl.ForceInlineSpellCheckAsync(token);
    });
    private async void BuiltIn_Click(object? sender, RoutedEventArgs e) => await RunAsync(async token =>
    {
        RequireDesign();
        EditorControl.SpellCheckOptions.SpellChecker = SpellCheckerEngineTypes.OpenOffice;
        await EditorControl.ForceInlineSpellCheckAsync(token);
    });
    private async void Review_Click(object? sender, RoutedEventArgs e) => await RunAsync(token => EditorControl.Dialog.ShowSpellCheckerDialogAsync(token));

}
