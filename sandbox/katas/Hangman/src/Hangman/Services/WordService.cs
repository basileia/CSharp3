using Hangman.Interfaces;
using Hangman.Models;

namespace Hangman.Services
{
    public class WordService : IWordService
    {
        private readonly List<WordEntry> _words = new();

        public WordService(string filePath)
        {
            LoadWords(filePath);
        }

        private void LoadWords(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"[Warning] CSV soubor '{filePath}' nebyl nalezen.");
                    return;
                }

                var lines = File.ReadAllLines(filePath);

                if (lines.Length <= 1)
                {
                    Console.WriteLine($"[Warning] CSV soubor '{filePath}' neobsahuje žádná slova.");
                    return;
                }

                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split(',');

                    if (parts.Length >= 2 && !string.IsNullOrWhiteSpace(parts[1]))
                    {
                        _words.Add(new WordEntry
                        {
                            Category = parts[0].Trim(),
                            Word = parts[1].Trim().ToUpper()
                        });
                    }
                    else
                    {
                        Console.WriteLine($"[Warning] Ignoruji řádek se špatným formátem: {line}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Chyba při načítání CSV: {ex.Message}");
            }
        }

        public List<string> GetCategories()
        {
            return _words.Select(w => w.Category).Distinct().ToList();
        }

        public string? GetRandomWord(string? category = null)
        {
            var query = string.IsNullOrEmpty(category)
                ? _words
                : _words.Where(w => w.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!query.Any())
            {
                Console.WriteLine("[Warning] Nebyla nalezena žádná slova pro danou kategorii.");
                return null;
            }

            var index = Random.Shared.Next(query.Count);
            return query[index].Word;
        }
    }
}
