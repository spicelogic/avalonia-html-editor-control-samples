# Custom dialogs

Supply a fresh branded window for each dialog opening and retain guarded document updates.

```shell
dotnet run --project 08-CustomDialog/08-CustomDialog.csproj
```

Run the command from the repository root, or choose this project as the startup project in `AvaloniaHtmlEditorSamples.sln`.

## Try it

1. Select some document text, open **Custom color picker**, choose a brand color and apply it. Reopen it and cancel to check the dismissal path.
2. Open **Style settings** and its color picker. The same factory handles the nested color request with the correct owner.
3. Open **Image properties**, **Hyperlink** or **Table** to see application titles on the stock dialogs.

The sample replaces the color window's content and customizes the other nine factories through `CreateDefaultDialog`. It covers image, hyperlink, style, spelling, symbol, table, table cell, search, video and color contracts. The default dialogs keep their full built-in validation and behavior.

Never show or cache a factory window yourself. Return a fresh unshown window; await `context.AcceptAsync` before closing after acceptance. A failed or stale selection update stays visible as an error, and closing the custom dialog cancels pending acceptance.

**Read HTML** exports the current complete document after applying any pending Source draft. No file is written and no message is sent.

## Read the code

- [MainWindow.axaml](MainWindow.axaml) declares the real editor and application actions.
- [MainWindow.axaml.cs](MainWindow.axaml.cs) contains this scenario's public API calls.
- [Shared startup and lifetime code](../Shared) handles readiness, cancellation and error display.

The samples reference the published `SpiceLogic.HtmlEditor.Avalonia` package. The sample code is MIT licensed; the editor has a separate commercial license. See the [repository guide](../README.md) for installation, platform requirements and licensing.
