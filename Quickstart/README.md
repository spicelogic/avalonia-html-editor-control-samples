# Quickstart

The smallest complete Avalonia host: one editor, a starting document and an awaited HTML export.

```shell
dotnet run --project Quickstart/Quickstart.csproj
```

Edit the document, then choose **Read current document HTML**. The output below the editor contains the complete current document, including Unicode characters. No file is written.

[Program.cs](Program.cs) shows startup, the Fluent theme, the native window and event handlers together. It waits for `Ready` after the window opens, disables export until initialization completes, displays failures and disposes the editor on close.

For an AXAML window and a bound view model, continue with [MVVM data binding](../02-MvvmDataBinding). For the combined integration examples, open [ScenarioGallery](../ScenarioGallery).

See the [repository guide](../README.md) for prerequisites and licensing.
