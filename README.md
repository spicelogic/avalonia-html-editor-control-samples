# SpiceLogic Avalonia HTML Editor samples

The quick start hosts the HTML editor in a normal Avalonia desktop window, waits for readiness, loads Unicode content and exports the current complete document through an awaited API. The scenario gallery demonstrates application integration with one editor and six focused examples.

Use .NET SDK 9.0.300 or newer (SDK 10 recommended), targeting .NET 8 or newer.
Avalonia 12.1.4's analyzers require the newer compiler:

```shell
dotnet run --project Quickstart/Quickstart.csproj
dotnet run --project ScenarioGallery/ScenarioGallery.csproj
```

Both projects reference `SpiceLogic.HtmlEditor.Avalonia` version `1.0.2` and Avalonia Desktop/Fluent `12.1.4`. Restore from a feed containing that package version. The control is commercially licensed separately from this sample code. Applications using a compiled commercial license assign their signed Avalonia key before creating controls:

```csharp
SpiceLogic.HtmlEditor.Avalonia.AvaloniaHtmlEditor.LicenseKey = "your-signed-Avalonia-key";
```

The control targets Avalonia desktop applications on Windows, macOS and Linux. Windows uses WebView2; macOS and Linux native operation has not yet been qualified. Install the native browser prerequisites for your platform: WebView2 on Windows, built-in WKWebView on macOS, and the supported WPE/WebKitGTK libraries on Linux. See [Avalonia's native WebView prerequisites](https://docs.avaloniaui.net/docs/app-development/embedding-web-content).

The sample does not write a file or contact a remote document service. The exported HTML appears below the editor so you can connect it to your application's own save workflow. Loaded HTML is sanitized and external resources are blocked by default.

Await `Ready` after the window opens. Use awaited getters for a fresh export; avoid blocking the UI thread with `Wait()` or `Result`. The editor includes its internal source-view styles, so no additional dependency theme registration is required.

## Scenario gallery

Open [ScenarioWindow.cs](ScenarioGallery/ScenarioWindow.cs) to follow the application code. [SampleDocumentViewModel.cs](ScenarioGallery/SampleDocumentViewModel.cs) is a plain `INotifyPropertyChanged` view model; the examples need no additional MVVM library.

| Example | Public contract demonstrated |
| --- | --- |
| Document binding and source | Two-way `DocumentHtml` binding, awaited `SetEditorModeAsync` with the shared `SpiceLogic.HtmlEditor.Abstractions.EditorModes` values `WysiwygDesign`, `HtmlEdit` and `ReadOnlyPreview`, source cancellation, `Options.FormatHtmlSource` with `SynchronizeOptionsAsync`, and the attached source editor's `WordWrap`. |
| Toolbar and context menu | `ToolbarItemOverrider.OverrideAsync` replaces the stock Save action with an application export. A native replacement `EditorContextMenuStrip` calls the real `Editor` and `Formatting` facades; null restores the built-in menu. |
| Typed dialog factories | `Dialog.CreateColorPickerDialogMethod` creates a fresh window, reads copied inputs and awaits guarded `AcceptAsync`. The same factory handles top-level and nested Style/Image colors. `CreateDefaultDialog` supports customization of a fresh built-in Style window. |
| Inline and review spelling | Inline checking, actual review, and an explicitly picked local Hunspell `.dic`/`.aff` pair through `SpellCheckOptions.DictionaryFile`. No dictionary downloads or user-word persistence are performed by this example. |
| Mail merge | Shared `Content.MailMerge.PlaceholderFields`, actual placeholder insertion, remaining-field discovery and literal-text merge values. |
| Body defaults and reapply | Configure document body defaults, preserve a subsequently loaded document and deliberately reapply the configured defaults. |
| Native appearance and language | Change the window's Light/Dark theme, the editor's English/German interface and its three stock toolbar icon sizes while retaining document styles and content. |

These are native asynchronous equivalents of familiar WPF integration scenarios. The facades use actual native controls and guarded browser acknowledgments. A custom dialog factory returns an unshown, newly constructed window every time; the control owns modal display and focus return. Application overrides are responsible for their own behavior and error presentation.

Source edits are a separate draft. `DocumentHtml` reflects accepted content, and an awaited content export applies a pending source draft before returning. Switching away from Source also applies it; Cancel source edits discards it. Binding both `BodyHtml` and `DocumentHtml` to independent inputs is unnecessary and can make those inputs compete. Source formatting and wrapping affect presentation; formatting option changes do not rewrite a dirty source draft. The source wrapping checkbox changes the real attached AvaloniaEdit control, while the editor's `WordWrap` property controls the authored document body.

The gallery displays errors and serializes its application actions while an operation is pending. Mode/read-only/resource guards still apply: selecting a command does not grant permission to load arbitrary local files or bypass the current editor state. Dictionary selection requires one local `.dic` file and one matching `.aff` file. Closing the application cancels pending work and disposes its editor.
