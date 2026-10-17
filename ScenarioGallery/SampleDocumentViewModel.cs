using System.ComponentModel;

namespace SpiceLogic.Samples.Avalonia;

// This view model knows nothing about native controls or browser transport.
internal sealed class SampleDocumentViewModel : INotifyPropertyChanged
{
    internal const string InitialDocument = "<!DOCTYPE html><html><head><title>Project update</title></head><body><h1>Project update</h1><p>Edit this document, then explore the examples on the left.</p><p>Unicode: café, 日本語, 🙂.</p></body></html>";
    private string _documentHtml = InitialDocument;

    public string DocumentHtml
    {
        get => _documentHtml;
        set
        {
            if (_documentHtml == value) return;
            _documentHtml = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DocumentHtml)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
