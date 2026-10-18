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
        // <quickstart>
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            AvaloniaHtmlEditor editor = new();
            CancellationTokenSource lifetime = new();
            CancellationToken token = lifetime.Token;
            bool closed = false;
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
            Window window = new() { Title = "SpiceLogic Avalonia HTML Editor quick start", Width = 1100, Height = 850,
                MinWidth = 1000, MinHeight = 640, WindowStartupLocation = WindowStartupLocation.CenterScreen, Content = layout };
            window.Opened += async (_, _) =>
            {
                try
                {
                    await editor.Ready.WaitAsync(TimeSpan.FromSeconds(30), token);
                    await editor.SetDocumentHtmlAsync("""
                        <!DOCTYPE html><html><head><title>Project update</title><style>
                        body { font-family:Arial; color:#263449; padding:20px 30px; line-height:1.6; }
                        h1 { color:#153c65; } blockquote { border-left:4px solid #008b8b; padding:12px 18px; background:#eef8f8; margin-left:0; }
                        </style></head><body><h1>Project update</h1>
                        <p>The content review is complete. The next milestone is the customer launch.</p>
                        <blockquote><strong>Ready for review:</strong> Product overview, onboarding messages and delivery notes.</blockquote>
                        <p>Our content supports customers around the world: café, 日本語, 🙂.</p></body></html>
                        """, token);
                    if (closed) return;
                    export.IsEnabled = true;
                    status.Text = "Document ready";
                }
                catch (OperationCanceledException) when (closed) { }
                catch (Exception error) { if (!closed) status.Text = error.Message; }
            };
            export.Click += async (_, _) =>
            {
                export.IsEnabled = false;
                try
                {
                    string current = await editor.GetDocumentHtmlAsync(token);
                    if (closed) return;
                    html.Text = current;
                    status.Text = "Current HTML read";
                }
                catch (OperationCanceledException) when (closed) { }
                catch (Exception error) { if (!closed) status.Text = error.Message; }
                finally { if (!closed) export.IsEnabled = true; }
            };
            window.Closed += (_, _) => { closed = true; lifetime.Cancel(); editor.Dispose(); lifetime.Dispose(); };
            desktop.MainWindow = window;
        }
        // </quickstart>
        base.OnFrameworkInitializationCompleted();
    }
}
