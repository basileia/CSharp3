namespace Hangman.Services
{
    public class HangmanGameService

    {
        public string SecretWord { get; private set; }
        public HashSet<char> GuessedLetters { get; private set; } = new();
        public int NumberOfMistakes { get; private set; } = 0;
        public int MaxMistakes { get; private set; }

        public HangmanGameService(string secretWord, int maxMistakes = 6)
        {
            if (string.IsNullOrWhiteSpace(secretWord))
                throw new ArgumentException("Hádané slovo nemůže být prázdné");

            SecretWord = secretWord.ToUpper();
            MaxMistakes = maxMistakes;
        }

        public (bool Success, string Message) Guess(char letter)
        {
            letter = char.ToUpper(letter);

            if (!char.IsLetter(letter))
                return (false, "Zadejte platné písmeno.");

            if (GuessedLetters.Contains(letter))
                return (false, "Toto písmeno už jsi zkoušel.");

            GuessedLetters.Add(letter);

            if (SecretWord.Contains(letter))
            {
                return (true, $"Písmeno '{letter}' je ve slově.");
            }
            else
            {
                NumberOfMistakes++;
                return (false, $"Písmeno '{letter}' ve slově není.");
            }
        }

        public string GetCurrentProgress()
        {
            return string.Concat(SecretWord.Select(c => GuessedLetters.Contains(c) ? c : '_'));
        }

        public bool IsGameOver()
        {
            return NumberOfMistakes >= MaxMistakes || IsWordGuessed();
        }

        public bool IsWordGuessed()
        {
            return SecretWord.All(c => GuessedLetters.Contains(c));
        }        
    }
}
