using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpiceLogic.HtmlEditor.Abstractions;
using SpiceLogic.HtmlEditor.Resources.Localization;
using SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck;
namespace SpiceLogic.Samples;

public partial class MainWindow : SampleWindow
{

    public MainWindow() { AvaloniaXamlLoader.Load(this); ConnectEditorEvents(); }
    private async void English_Click(object? sender, RoutedEventArgs e) => await SetLanguageAsync(EditorLanguage.EnglishUs);
    private async void German_Click(object? sender, RoutedEventArgs e) => await SetLanguageAsync(EditorLanguage.German);
    private async void French_Click(object? sender, RoutedEventArgs e) => await SetLanguageAsync(EditorLanguage.French);
    private Task SetLanguageAsync(EditorLanguage language) => RunAsync(_ =>
    { EditorControl.Language = language; return Task.CompletedTask; });
    private async void EnglishSpelling_Click(object? sender, RoutedEventArgs e) => await SetSpellingAsync(SpellCheckLanguage.EnglishUs);
    private async void GermanSpelling_Click(object? sender, RoutedEventArgs e) => await SetSpellingAsync(SpellCheckLanguage.German);
    private Task SetSpellingAsync(SpellCheckLanguage language) => RunAsync(async token =>
    {
        RequireDesign();
        EditorControl.SpellCheckOptions.SpellCheckLanguage = language;
        await EditorControl.ForceInlineSpellCheckAsync(token);
    });

}
