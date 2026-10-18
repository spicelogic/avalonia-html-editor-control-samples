# Toolbar customization

Wire the stock Save button to your application and offer your own formatting actions.

```shell
dotnet run --project 04-ToolbarCustomization/04-ToolbarCustomization.csproj
```

Run the command from the repository root, or choose this project as the startup project in `AvaloniaHtmlEditorSamples.sln`.

## Try it

1. Select text in the document and choose **Bold selection** or **Italic selection**. The application buttons call the editor's real formatting service.
2. Choose **Hide Print**, then **Restore Print**. The actual factory button changes visibility without rebuilding the toolbar.
3. Choose the editor's stock Save button. Its overridden action reads the document into the output panel instead of opening a file picker.

`ToolbarItemOverrider.OverrideAsync` changes the stock action; `ToolbarItems.Print` exposes the native button. The custom action row is declared in AXAML. Commands retain the editor's mode and selection guards.

**Read HTML** exports the current complete document after applying any pending Source draft. No file is written and no message is sent.

## Read the code

- [MainWindow.axaml](MainWindow.axaml) declares the real editor and application actions.
- [MainWindow.axaml.cs](MainWindow.axaml.cs) contains this scenario's public API calls.
- [Shared startup and lifetime code](../Shared) handles readiness, cancellation and error display.

The samples reference the published `SpiceLogic.HtmlEditor.Avalonia` package. The sample code is MIT licensed; the editor has a separate commercial license. See the [repository guide](../README.md) for installation, platform requirements and licensing.
