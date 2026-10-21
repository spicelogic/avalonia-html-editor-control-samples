# Avalonia HTML Editor Control samples

[![NuGet](https://img.shields.io/nuget/v/SpiceLogic.HtmlEditor.Avalonia.svg)](https://www.nuget.org/packages/SpiceLogic.HtmlEditor.Avalonia)
[![Sample license: MIT](https://img.shields.io/badge/sample%20code-MIT-blue.svg)](LICENSE)

**Ten runnable C# examples for adding a WYSIWYG HTML editor to an Avalonia desktop application.** Start with a complete editor, then explore MVVM binding, custom toolbars, email templates, spelling and native dialogs in separate projects you can read and adapt.

[Product](https://spicelogic.com/products/avalonia-html-editor-control-35) | [Documentation](https://spicelogic.com/docs/avalonia-html-editor-control) | [NuGet](https://www.nuget.org/packages/SpiceLogic.HtmlEditor.Avalonia)

## Start here

```shell
git clone https://github.com/spicelogic/avalonia-html-editor-control-samples.git
cd avalonia-html-editor-control-samples
dotnet build AvaloniaHtmlEditorSamples.sln
dotnet run --project Quickstart/Quickstart.csproj
```

In Visual Studio or Rider, open **AvaloniaHtmlEditorSamples.sln** and choose an example as the startup project. Run one sample at a time. The eight focused feature windows declare the editor in `MainWindow.axaml`; Quickstart keeps the smallest complete host in one file, and ScenarioGallery combines the workflows in one application.

### Requirements

- .NET SDK **9.0.300 or newer**, with SDK 10 recommended. The samples target .NET 8; Avalonia 12.1.4's analyzers need the newer compiler.
- Windows 11 x64 with the **Microsoft Edge WebView2 Runtime** for the qualified native configuration.
- Internet access for the first NuGet restore. After packages are cached, the examples do not need a document server.

The control targets Avalonia desktop applications, with Windows 11 x64 as the qualified release platform. A separate native suite checks Ubuntu 24.04 Xorg; macOS qualification is deferred. See the [platform requirements](https://spicelogic.com/docs/avalonia-html-editor-control).

## Choose a sample

| # | Project | What you learn |
| --- | --- | --- |
| 01 | [Quickstart](Quickstart) | Host one editor, wait for readiness, load Unicode content and read current HTML. |
| 02 | [MVVM data binding](02-MvvmDataBinding) | Bind `DocumentHtml` to an `INotifyPropertyChanged` view model, load model content and apply or cancel a Source draft. |
| 03 | [Theme and appearance](03-ThemeAndAppearance) | Switch Light/Dark themes, change toolbar icon size and deliberately reapply document body defaults. |
| 04 | [Toolbar customization](04-ToolbarCustomization) | Override stock Save, hide and restore a built-in button, and call formatting from your own toolbar. |
| 05 | [Mail merge templates](05-MailMergeTemplate) | Insert named fields, discover outstanding fields, merge literal customer values and undo the merge. |
| 06 | [Localization](06-Localization) | Change English, German and French editor UI independently of the spelling dictionary. |
| 07 | [Custom spell checker](07-CustomSpellChecker) | Implement `ISpellCheckerEngine`, provide suggestions and switch between your vocabulary and bundled dictionaries. |
| 08 | [Custom dialogs](08-CustomDialog) | Build a color picker and customize all ten public dialog factories, including nested colors. |
| 09 | [Custom context menu](09-CustomContextMenu) | Build a right-click menu using current selection and read-only state, then restore the default menu. |
| 10 | [Full scenario gallery](ScenarioGallery) | Explore binding, HTML/XHTML/XML export, unsent email, local dictionaries, templates, dialogs and appearance together. |

The topics mirror our [WPF sample collection](https://github.com/spicelogic/wpf-html-editor-control-samples). These C# Avalonia examples use the actual asynchronous API. The WPF repository's VB.NET twins are not included here.

## Add the control to your application

```shell
dotnet add package SpiceLogic.HtmlEditor.Avalonia --version 1.0.5
```

Declare the editor in an Avalonia window:

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:editor="using:SpiceLogic.HtmlEditor.Avalonia">
    <editor:AvaloniaHtmlEditor x:Name="Editor" />
</Window>
```

After the owner window has opened:

```csharp
var editor = this.FindControl<SpiceLogic.HtmlEditor.Avalonia.AvaloniaHtmlEditor>("Editor")!;
await editor.Ready;
await editor.SetBodyHtmlAsync("<h1>Project update</h1><p>Ready for review.</p>");

// An awaited export includes the latest edits and applies a pending Source draft.
string htmlToSave = await editor.GetDocumentHtmlAsync();
```

[Quickstart/Program.cs](Quickstart/Program.cs) includes complete startup, event handling, error display and disposal. The control includes its source editor's styles; no extra source-view theme registration is needed.

### Integration rules

- **Show the window before awaiting `Ready`.** The native browser needs its owner. Never block the UI thread with `.Wait()` or `.Result`.
- **Await the value you intend to save.** A bound property represents application state; an awaited content export is the save boundary. Source is a separate draft until export, Apply or a mode transition accepts it.
- **Bind one authoring property.** Independent two-way inputs for both `BodyHtml` and `DocumentHtml` can compete. The MVVM sample binds only `DocumentHtml`.
- **Return a fresh, unshown dialog from a factory.** The control owns modal display and safe selection/focus handling.
- **Handle cancellation and failures.** The focused samples serialize their application commands, display errors and cancel pending work on close.
- **Handle resources explicitly.** Loaded HTML is sanitized and external resources are blocked by default. A sample action does not grant arbitrary file or network access.

## What you can build

Use the editor for customer messages, report descriptions, rich notes, knowledge-base content and reusable HTML templates. The default control supplies formatting, tables, images, links, Word paste cleanup, source view and spelling. Your application owns persistence, mail delivery and business rules.

`Read HTML` displays the export without writing a file. The gallery's email example creates an unsent `MailMessage`. The custom spelling example uses a small demonstration vocabulary to teach the integration contract.

## Working with a coding assistant

Connect the documentation MCP server:

```text
https://mcp.spicelogic.com/html-editor/avalonia
```

For Claude Code:

```shell
claude mcp add --transport http spicelogic-avalonia https://mcp.spicelogic.com/html-editor/avalonia
```

Use the current documentation and API signatures when adapting an example. Client configuration varies; the [documentation](https://spicelogic.com/docs/avalonia-html-editor-control) includes the MCP setup guide.

## Trial and licensing

Run the samples without a key to evaluate the control in trial mode. The **sample code is MIT licensed**; the editor is a separate commercial product.

Applications using a compiled-control license assign their signed Avalonia key once, before constructing any editor:

```csharp
SpiceLogic.HtmlEditor.Avalonia.AvaloniaHtmlEditor.LicenseKey = "your-avalonia-license-key";
```

Place this in application startup before creating the first window. Never commit a real key. Source-license customers follow their source delivery's licensing instructions.

[Pricing and licenses](https://spicelogic.com/products/avalonia-html-editor-control-35/pricing) | [Your account](https://members.spicelogic.com) | [Public download](https://spicelogic.com/products/avalonia-html-editor-control-35/download)

## Repository layout

`Directory.Build.props` pins editor and Avalonia versions in one place. `Shared` contains linked startup, a starting document and readiness/error handling, without implementing editor behavior. Each focused project's code-behind contains its scenario.

Build the whole solution after changing a package version. Every project consumes NuGet packages, without a reference to private product source or a bundled license key.
