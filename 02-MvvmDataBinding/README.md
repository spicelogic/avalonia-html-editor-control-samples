# MVVM data binding

Keep your view model and document in step, including edits made in Source.

```shell
dotnet run --project 02-MvvmDataBinding/02-MvvmDataBinding.csproj
```

Run the command from the repository root, or choose this project as the startup project in `AvaloniaHtmlEditorSamples.sln`.

## Try it

1. Choose **Load from view model**. The brief changes from Northfield to Lakeside through `DocumentHtml` binding.
2. Edit the document, then choose **Read current model**. The output shows both the awaited export and the bound value.
3. Switch to **Source**, add a paragraph and choose **Read current model**. Export accepts the pending draft before returning.
4. Make another Source edit and choose **Cancel source edits**. The unaccepted draft is discarded.
5. Choose **Design** to return to visual editing.

[DocumentViewModel.cs](DocumentViewModel.cs) has no dependency on native controls or an MVVM framework. Use the awaited export when saving; do not assume an arbitrary read of the bound property flushes a pending source draft.

**Read HTML** exports the current complete document after applying any pending Source draft. No file is written and no message is sent.

## Read the code

- [MainWindow.axaml](MainWindow.axaml) declares the real editor and application actions.
- [MainWindow.axaml.cs](MainWindow.axaml.cs) contains this scenario's public API calls.
- [Shared startup and lifetime code](../Shared) handles readiness, cancellation and error display.

The samples reference the published `SpiceLogic.HtmlEditor.Avalonia` package. The sample code is MIT licensed; the editor has a separate commercial license. See the [repository guide](../README.md) for installation, platform requirements and licensing.
