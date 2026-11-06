# Localization

Choose the editor interface language independently of the spelling dictionary.

```shell
dotnet run --project 06-Localization/06-Localization.csproj
```

Run the command from the repository root, or choose this project as the startup project in `AvaloniaHtmlEditorSamples.sln`.

## Try it

1. With the default English interface, hover over Bold and open the hyperlink dialog. The shipped JSON changes them to "Emphasize" and "Add a web link".
2. Choose **German interface** or **French interface** and inspect the editor's toolbar tooltips and built-in dialogs.
3. Choose **English spelling** while keeping the German interface. Choosing a spelling language pins it, so it no longer follows `Language`.
4. Choose **German spelling** to rescan with that bundled dictionary, then return to **English interface** as needed.

The sample's application buttons remain English because they belong to the host application. `EditorLanguage` changes the editor UI, not your application culture or authored text. No dictionary download is required for the bundled choices.

## Find and customize every label

The [localization folder](SpiceLogic.HtmlEditor.Localization) contains the complete
[JSON key catalog](SpiceLogic.HtmlEditor.Localization/_OVERRIDABLE-KEYS.reference.json),
an [English override example](SpiceLogic.HtmlEditor.Localization/EditorStrings.en-US.json),
and the [download, edit, load and rerun instructions](SpiceLogic.HtmlEditor.Localization/README.md).
Use the catalog to find the actual key, then include only the entries you want to
change in your culture file. The sample loads its shipped English override before
constructing the editor, so its changed wording appears immediately.

**Read HTML** exports the current complete document after applying any pending Source draft. No file is written and no message is sent.

## Read the code

- [MainWindow.axaml](MainWindow.axaml) declares the real editor and application actions.
- [MainWindow.axaml.cs](MainWindow.axaml.cs) contains this scenario's public API calls.
- [Shared startup and lifetime code](../Shared) handles readiness, cancellation and error display.

The samples reference the published `SpiceLogic.HtmlEditor.Avalonia` package. The sample code is MIT licensed; the editor has a separate commercial license. See the [repository guide](../README.md) for installation, platform requirements and licensing.

## A custom interface language

Select `Custom` and load `SpiceLogic.HtmlEditor.Localization` to use
`EditorStrings.custom.json`. WinForms and WPF expose the JSON override checkbox;
Avalonia loads the folder at startup and provides a Custom interface button.
The supplied file demonstrates two Turkish labels. Remaining labels fall back
to neutral English. For a complete language, ask your coding assistant to translate
every value in `_OVERRIDABLE-KEYS.reference.json`, preserving the keys and format
placeholders, and save the result as `EditorStrings.custom.json`. Review the
translation and test the actual dialogs before shipping. Custom does not supply
a spelling dictionary or infer a .NET culture; configure spelling separately.
