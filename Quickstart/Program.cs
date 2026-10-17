using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Themes.Fluent;
using SpiceLogic.HtmlEditor.Avalonia;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args) => AppBuilder.Configure<QuickstartApplication>()
        .UsePlatformDetect().LogToTrace().StartWithClassicDesktopLifetime(args);
}

internal sealed class QuickstartApplication : Application
{
    public override void Initialize() => Styles.Add(new FluentTheme());

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            AvaloniaHtmlEditor editor = new();
            TextBlock status = new() { Text = "Starting the editor" };
            Button export = new() { Content = "Read current document HTML", IsEnabled = false };
            TextBox html = new() { IsReadOnly = true, AcceptsReturn = true, TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            StackPanel actions = new() { Orientation = Orientation.Horizontal, Spacing = 12, Margin = new Thickness(12) };
            actions.Children.Add(export);
            actions.Children.Add(status);
            Grid layout = new() { RowDefinitions = new RowDefinitions("Auto,*,180") };
            layout.Children.Add(actions);
            Grid.SetRow(editor,1);
            layout.Children.Add(editor);
            Grid.SetRow(html,2);
            layout.Children.Add(html);
            Window window = new() { Title = "SpiceLogic Avalonia HTML Editor quick start", Width = 1100, Height = 850, Content = layout };
            window.Opened += async (_, _) =>
            {
                try
                {
                    await editor.Ready;
                    await editor.SetBodyHtmlAsync("<h1>Project update</h1><p>Edit this content, then read the current HTML.</p><p>Unicode: café, 日本語, 🙂.</p>");
                    export.IsEnabled = true;
                    status.Text = "Document ready";
                }
                catch (Exception error) { status.Text = error.Message; }
            };
            export.Click += async (_, _) =>
            {
                export.IsEnabled = false;
                try
                {
                    html.Text = await editor.GetDocumentHtmlAsync();
                    status.Text = "Current HTML read";
                }
                catch (Exception error) { status.Text = error.Message; }
                finally { export.IsEnabled = true; }
            };
            window.Closed += (_, _) => editor.Dispose();
            desktop.MainWindow = window;
        }
        base.OnFrameworkInitializationCompleted();
    }
}
