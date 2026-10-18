using System.ComponentModel;
namespace SpiceLogic.Samples;

public sealed class DocumentViewModel : INotifyPropertyChanged
{
    private string _documentHtml = SampleDocuments.ProjectBrief;
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
