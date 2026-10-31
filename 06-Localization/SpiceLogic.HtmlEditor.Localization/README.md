# Customize the editor's wording

This folder contains the complete JSON key catalog and an example culture override.
Download the repository with **Code > Download ZIP**, or clone it, then open this
localization sample in Visual Studio.

1. Open `_OVERRIDABLE-KEYS.reference.json` and search for the text shown in the
   editor. It contains every overridable resource key with its default English text.
2. Copy only the keys you need into `EditorStrings.<culture>.json`. The supplied
   `EditorStrings.en-US.json` changes the English Bold tooltip to "Emphasize" and
   the hyperlink dialog title to "Add a web link". Other text keeps its built-in
   translation. Match the file's culture to the editor language, for example
   `en-US`, `de-DE`, `fr-FR` or `pl-PL`.
3. Before loading the window's XAML, load the folder once in `MainWindow`:

   ```csharp
   LocalizationManager.SetJsonOverrideDirectory(
       Path.Combine(AppContext.BaseDirectory, "SpiceLogic.HtmlEditor.Localization"));
   AvaloniaXamlLoader.Load(this);
   ConnectEditorEvents();
   ```

   `LocalizationManager` is in `SpiceLogic.HtmlEditor.Resources.Localization`.
   Add `using System.IO;` if the project does not import it globally.
4. Run with the English interface, hover over Bold, and open the hyperlink dialog
   to see the changed text. Edit a JSON value and rerun to verify your own wording.
5. Ship the culture override beside your application. The project copies the
   supplied English override to its output; give any added culture files the same
   **Copy to Output Directory** setting.

The reference catalog is documentation, not a culture file, and is not loaded at
runtime. Do not rename it to an active culture file or deploy all its entries
when only a few labels need changing. JSON overrides use a flat object of string
key/value pairs. The built-in translations remain available without these files.
