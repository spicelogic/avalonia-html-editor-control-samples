# Localization

Choose the editor interface language independently of the spelling dictionary.

```shell
dotnet run --project 06-Localization/06-Localization.csproj
```

Run the command from the repository root, or choose this project as the startup project in `AvaloniaHtmlEditorSamples.sln`.

## Try it

1. Choose **German interface** or **French interface** and inspect the editor's toolbar tooltips and built-in dialogs.
2. Choose **English spelling** while keeping the German interface. The spelling language is independent of `Language`.
3. Choose **German spelling** to rescan with that bundled dictionary, then return to **English interface** as needed.

The sample's application buttons remain English because they belong to the host application. `EditorLanguage` changes the editor UI, not your application culture or authored text. No dictionary download is required for the bundled choices.

**Read HTML** exports the current complete document after applying any pending Source draft. No file is written and no message is sent.

## Read the code

- [MainWindow.axaml](MainWindow.axaml) declares the real editor and application actions.
- [MainWindow.axaml.cs](MainWindow.axaml.cs) contains this scenario's public API calls.
- [Shared startup and lifetime code](../Shared) handles readiness, cancellation and error display.

The samples reference the published `SpiceLogic.HtmlEditor.Avalonia` package. The sample code is MIT licensed; the editor has a separate commercial license. See the [repository guide](../README.md) for installation, platform requirements and licensing.
