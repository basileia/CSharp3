namespace Hangman.Interfaces
{
    public interface IWordService
    {
        List<string> GetCategories();
        string? GetRandomWord(string? category = null);
    }
}
