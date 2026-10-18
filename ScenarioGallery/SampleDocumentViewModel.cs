using System.ComponentModel;

namespace SpiceLogic.Samples.Avalonia;

// This view model knows nothing about native controls or browser transport.
internal sealed class SampleDocumentViewModel : INotifyPropertyChanged
{
    internal const string InitialDocument = global::SpiceLogic.Samples.SampleDocuments.ProjectBrief;
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
