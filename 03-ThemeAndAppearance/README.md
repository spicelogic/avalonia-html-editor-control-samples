# Theme and appearance

Match the application theme while keeping the authored document intact.

```shell
dotnet run --project 03-ThemeAndAppearance/03-ThemeAndAppearance.csproj
```

Run the command from the repository root, or choose this project as the startup project in `AvaloniaHtmlEditorSamples.sln`.

## Try it

1. Switch **Dark theme** and **Light theme**. The application chrome changes while the authored document retains its colors.
2. Switch **Small icons** and **Large icons**. These change native toolbar controls, not the document zoom.
3. Choose **Apply body defaults** in Design mode. The sample configures Georgia, 14pt and teal, awaits synchronization and deliberately reapplies those defaults.
4. Use the editor's Undo button to undo the document styling operation.

`RequestedThemeVariant` belongs to this window. `DefaultFontFamily`, `DefaultFontSizeInPt` and `DefaultForeColor` describe authored body styles; they are a separate choice from the application theme.

**Read HTML** exports the current complete document after applying any pending Source draft. No file is written and no message is sent.

## Read the code

- [MainWindow.axaml](MainWindow.axaml) declares the real editor and application actions.
- [MainWindow.axaml.cs](MainWindow.axaml.cs) contains this scenario's public API calls.
- [Shared startup and lifetime code](../Shared) handles readiness, cancellation and error display.

The samples reference the published `SpiceLogic.HtmlEditor.Avalonia` package. The sample code is MIT licensed; the editor has a separate commercial license. See the [repository guide](../README.md) for installation, platform requirements and licensing.
