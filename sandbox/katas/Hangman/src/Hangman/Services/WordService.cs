using Hangman.Interfaces;
using Hangman.Models;

namespace Hangman.Services
{
    public class WordService : IWordService
    {
        private readonly List<WordEntry> _words = new();
        private readonly ILogger<WordService> _logger;

        public WordService(string filePath, ILogger<WordService> logger)
        {
            _logger = logger;
            LoadWords(filePath);
        }

        private void LoadWords(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("CSV file '{FilePath}' was not found.", filePath);
                    return;
                }

                var lines = File.ReadAllLines(filePath);

                if (lines.Length <= 1)
                {
                    _logger.LogWarning("CSV file '{FilePath}' does not contain any words.", filePath);
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
                        _logger.LogWarning("Skipping invalid line format: {Line}", line);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the CSV file.");
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
                _logger.LogWarning("No words were found for category '{Category}'.", category);
                return null;
            }

            var index = Random.Shared.Next(query.Count);
            return query[index].Word;
        }
    }
}
