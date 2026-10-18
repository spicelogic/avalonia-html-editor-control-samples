using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;

namespace SpiceLogic.Samples;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        // Assign AvaloniaHtmlEditor.LicenseKey here, before creating any window,
        // if your application has a compiled-control license. No key is needed
        // to evaluate the samples in trial mode. Never commit a real key.
        AppBuilder.Configure<SampleApplication>().UsePlatformDetect()
            .LogToTrace().StartWithClassicDesktopLifetime(args);
    }
}

internal sealed class SampleApplication : Application
{
    public override void Initialize() => Styles.Add(new FluentTheme());
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();
        base.OnFrameworkInitializationCompleted();
    }
}
