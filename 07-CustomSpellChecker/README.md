# Custom spell checker

Connect your own vocabulary and suggestions to inline spelling and the review dialog.

```shell
dotnet run --project 07-CustomSpellChecker/07-CustomSpellChecker.csproj
```

Run the command from the repository root, or choose this project as the startup project in `AvaloniaHtmlEditorSamples.sln`.

## Try it

1. Choose **Use custom vocabulary**. The deliberately misspelled `delivrey` is flagged while Northfield and onboarding are accepted.
2. Choose **Review spelling** and inspect the `delivery` suggestion. Apply it through the real spelling dialog.
3. Choose **Use bundled dictionary** to return to the normal Hunspell engine.

[ProjectSpellChecker.cs](ProjectSpellChecker.cs) implements initialization, lookup, suggestions, session-only word additions and disposal. Its vocabulary is intentionally small and thread-safe. It teaches the extension contract rather than providing a general-purpose dictionary. No words are written to disk and no service is contacted.

**Read HTML** exports the current complete document after applying any pending Source draft. No file is written and no message is sent.

## Read the code

- [MainWindow.axaml](MainWindow.axaml) declares the real editor and application actions.
- [MainWindow.axaml.cs](MainWindow.axaml.cs) contains this scenario's public API calls.
- [Shared startup and lifetime code](../Shared) handles readiness, cancellation and error display.

The samples reference the published `SpiceLogic.HtmlEditor.Avalonia` package. The sample code is MIT licensed; the editor has a separate commercial license. See the [repository guide](../README.md) for installation, platform requirements and licensing.
