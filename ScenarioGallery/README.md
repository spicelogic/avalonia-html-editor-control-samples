# Full scenario gallery

Explore several integrations in one application using the real editor's public asynchronous API.

```shell
dotnet run --project ScenarioGallery/ScenarioGallery.csproj
```

Expand a section on the left and try its actions. The output panel shows actual exports, while the document remains available for editing.

| Section | Try it |
| --- | --- |
| Document binding and source | Switch Design, Source and Preview. Edit Source, then read HTML, XHTML or XML. Cancel Source to discard an unaccepted draft. |
| Toolbar and context menu | Use stock Save to export, try the custom right-click menu and restore the built-in menu. |
| Typed dialog factories | Open the custom color picker directly or from Style. Each opening constructs a fresh window. |
| Inline and review spelling | Review spelling or select a matching local Hunspell `.dic` and `.aff` pair. Restore the bundled dictionary afterward. |
| Mail merge | Insert a customer field, merge its value and inspect remaining fields. Angle brackets in values remain text. |
| Body defaults and reapply | Configure defaults, load an authored Georgia document, reapply defaults deliberately and undo. |
| Native appearance and language | Change theme, interface language and icon size while preserving the document. |

Image and email actions honor resource permissions. Preparing email creates and disposes an unsent `MailMessage`; your application supplies recipients and delivery. The gallery does not send mail, download dictionaries or persist user words.

[ScenarioWindow.cs](ScenarioWindow.cs) contains the actions. [SampleDocumentViewModel.cs](SampleDocumentViewModel.cs) is a plain `INotifyPropertyChanged` view model. For smaller examples, use the [focused projects](../README.md#choose-a-sample).

See the [repository guide](../README.md) for prerequisites and licensing.
