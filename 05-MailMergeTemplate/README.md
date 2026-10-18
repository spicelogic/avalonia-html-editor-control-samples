# Mail merge templates

Create a reusable message, insert named fields and merge customer values as literal text.

```shell
dotnet run --project 05-MailMergeTemplate/05-MailMergeTemplate.csproj
```

Run the command from the repository root, or choose this project as the startup project in `AvaloniaHtmlEditorSamples.sln`.

## Try it

1. Place the caret in the greeting and choose **Insert customer field**.
2. Place it in the message body and choose **Insert project field**. Both fields are also registered with the built-in placeholder interface.
3. Choose **List remaining fields** to inspect the placeholders present in the document.
4. Choose **Merge sample values**. The customer value `Café <North>` appears as literal text, and the project becomes `Autumn launch`.
5. Choose **Undo merge** to return to the template.

`Content.MailMerge` uses field display names as dictionary keys. This example deliberately merges into the open document and supports Undo; retain your template separately when generating multiple customer documents.

**Read HTML** exports the current complete document after applying any pending Source draft. No file is written and no message is sent.

## Read the code

- [MainWindow.axaml](MainWindow.axaml) declares the real editor and application actions.
- [MainWindow.axaml.cs](MainWindow.axaml.cs) contains this scenario's public API calls.
- [Shared startup and lifetime code](../Shared) handles readiness, cancellation and error display.

The samples reference the published `SpiceLogic.HtmlEditor.Avalonia` package. The sample code is MIT licensed; the editor has a separate commercial license. See the [repository guide](../README.md) for installation, platform requirements and licensing.
