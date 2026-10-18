using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpiceLogic.HtmlEditor.Abstractions;
using Avalonia.Layout;
using Avalonia.Media;
using SpiceLogic.HtmlEditor.Avalonia.Models.Dialogs;
namespace SpiceLogic.Samples;

public partial class MainWindow : SampleWindow
{

    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this); ConnectEditorEvents();
        // A factory returns a NEW, unshown window. The control owns ShowDialog,
        // selection guards, cancellation and focus return, including nested colors.
        EditorControl.Dialog.CreateColorPickerDialogMethod = CreateColorWindow;
        EditorControl.Dialog.CreateImageDialogMethod = c => Brand(c.CreateDefaultDialog(), "Image");
        EditorControl.Dialog.CreateHyperlinkDialogMethod = c => Brand(c.CreateDefaultDialog(), "Hyperlink");
        EditorControl.Dialog.CreateStyleBuilderDialogMethod = c => Brand(c.CreateDefaultDialog(), "Style");
        EditorControl.Dialog.CreateSpellcheckerDialogMethod = c => Brand(c.CreateDefaultDialog(), "Spelling");
        EditorControl.Dialog.CreateSymbolDialogMethod = c => Brand(c.CreateDefaultDialog(), "Symbol");
        EditorControl.Dialog.CreateTableDialogMethod = c => Brand(c.CreateDefaultDialog(), "Table");
        EditorControl.Dialog.CreateTableCellDialogMethod = c => Brand(c.CreateDefaultDialog(), "Table cell");
        EditorControl.Dialog.CreateSearchDialogMethod = c => Brand(c.CreateDefaultDialog(), "Find and replace");
        EditorControl.Dialog.CreateYouTubeVideoInsertDialogMethod = c => Brand(c.CreateDefaultDialog(), "Video");
    }
    private static Window Brand(Window window, string purpose)
    { window.Title = "Northfield / " + purpose; return window; }
    private Window CreateColorWindow(ColorPickerDialogContext context)
    {
        var window = new Window { Title = "Northfield / Brand colors", Width = 400, Height = 340,
            CanResize = false, WindowStartupLocation = WindowStartupLocation.CenterOwner };
        var colors = new ComboBox { ItemsSource = new[] { context.InitialValue.ToString(), "#153c65", "#008b8b", "#b42318" }, SelectedIndex = 0 };
        var error = new TextBlock { TextWrapping = TextWrapping.Wrap };
        var apply = new Button { Content = "Apply color", HorizontalAlignment = HorizontalAlignment.Stretch };
        var cancel = new Button { Content = "Cancel", HorizontalAlignment = HorizontalAlignment.Stretch };
        window.Content = new StackPanel { Margin = new Thickness(24), Spacing = 16, Children =
        { new TextBlock { Text = "Choose a brand color", FontSize = 22 }, colors, error, apply, cancel } };
        var lifetime = CancellationTokenSource.CreateLinkedTokenSource(LifetimeToken);
        CancellationToken token = lifetime.Token;
        bool closed = false;
        window.Closed += (_, _) => { closed = true; lifetime.Cancel(); lifetime.Dispose(); };
        apply.Click += async (_, _) =>
        {
            if (closed || IsClosed) return;
            apply.IsEnabled = false;
            try
            {
                await context.AcceptAsync(Color.Parse((string)colors.SelectedItem!), token);
                if (!closed) window.Close();
            }
            catch (Exception failure) { if (!closed) { error.Text = failure.Message; apply.IsEnabled = true; } }
        };
        cancel.Click += (_, _) => window.Close();
        return window;
    }
    private async void Color_Click(object? sender, RoutedEventArgs e) => await RunAsync(token => EditorControl.Dialog.ShowColorPickerDialogAsync(cancellationToken: token));
    private async void Style_Click(object? sender, RoutedEventArgs e) => await RunAsync(token => EditorControl.Dialog.ShowStyleBuilderDialogAsync(token));
    private async void Image_Click(object? sender, RoutedEventArgs e) => await RunAsync(token => EditorControl.Dialog.ShowImageDialogAsync(token));
    private async void Link_Click(object? sender, RoutedEventArgs e) => await RunAsync(token => EditorControl.Dialog.ShowHyperlinkDialogAsync(token));
    private async void Table_Click(object? sender, RoutedEventArgs e) => await RunAsync(token => EditorControl.Dialog.ShowTableDialogAsync(cancellationToken: token));

}
