using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;

namespace SpiceLogic.Samples.Avalonia;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args) => AppBuilder.Configure<GalleryApplication>()
        .UsePlatformDetect().LogToTrace().StartWithClassicDesktopLifetime(args);
}

internal sealed class GalleryApplication : Application
{
    public override void Initialize() => Styles.Add(new FluentTheme());

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new ScenarioWindow();
        base.OnFrameworkInitializationCompleted();
    }
}
