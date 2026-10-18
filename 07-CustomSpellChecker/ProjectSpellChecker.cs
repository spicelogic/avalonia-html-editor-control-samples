using SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck;
namespace SpiceLogic.Samples;

// Deliberately small teaching vocabulary. Replace this with your own engine or
// dictionary service in production. There is no network or user-file persistence.
public sealed class ProjectSpellChecker : ISpellCheckerEngine
{
    private readonly HashSet<string> _words = new(
        "project vocabulary the northfield release is ready review delivery plan and onboarding note custom engine accepts our suggests for".Split(' '),
        StringComparer.OrdinalIgnoreCase);
    private readonly object _gate = new();
    public void Initialize(string dictionaryPath, string affixPath, string userDictionaryPath) { }
    public bool Spell(string word) { lock (_gate) return _words.Contains(word); }
    public IEnumerable<string> Suggest(string word, int? max = null) =>
        word.Equals("delivrey", StringComparison.OrdinalIgnoreCase) && max is not <= 0
            ? new[] { "delivery" } : Array.Empty<string>();
    public void AddToUserDictionary(string word) { lock (_gate) _words.Add(word); }
    public void Dispose() { }
}
