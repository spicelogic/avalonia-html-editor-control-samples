# Custom context menu

Build a right-click menu whose commands follow the current selection and editing mode.

```shell
dotnet run --project 09-CustomContextMenu/09-CustomContextMenu.csproj
```

Run the command from the repository root, or choose this project as the startup project in `AvaloniaHtmlEditorSamples.sln`.

## Try it

1. Select document text and right-click. The custom menu offers Copy selection, Bold selection and Read current HTML.
2. Right-click without a text selection. Copy selection becomes unavailable.
3. Switch to **Preview** and right-click. Bold is unavailable because the document is read-only.
4. Choose **Use built-in menu** to restore the editor's contextual menu. Choose **Design** and **Use custom menu** to return.

`ContextMenuShowing` supplies the actual opening's snapshot. The sample uses its selection and editing state before assigning `args.ContextMenu`, so it does not rely on cached state from an earlier click. The commands still call guarded public editor services.

**Read HTML** exports the current complete document after applying any pending Source draft. No file is written and no message is sent.

## Read the code

- [MainWindow.axaml](MainWindow.axaml) declares the real editor and application actions.
- [MainWindow.axaml.cs](MainWindow.axaml.cs) contains this scenario's public API calls.
- [Shared startup and lifetime code](../Shared) handles readiness, cancellation and error display.

The samples reference the published `SpiceLogic.HtmlEditor.Avalonia` package. The sample code is MIT licensed; the editor has a separate commercial license. See the [repository guide](../README.md) for installation, platform requirements and licensing.
