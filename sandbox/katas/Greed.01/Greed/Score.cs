namespace Greed;

public static class Score
{
    private static readonly Dictionary<int, int> TripleScores = new Dictionary<int, int>
    {
        {1, 1000},
        {2, 200},
        {3, 300},
        {4, 400},
        {5, 500},
        {6, 600}
    };

    private const int SingleOne = 100;
    private const int SingleFive = 50;

    public static int CalculateRollScore(List<int> numbers)
    {
        var numberCounts = CountNumbers(numbers);
        int score = 0;
        score += CalculateTriples(numberCounts);
        score += CalculateSingles(numberCounts);

        return score;
    }

    private static Dictionary<int, int> CountNumbers(List<int> numbers)
    {
        var numberCounts = new Dictionary<int, int>();

        foreach (int number in numbers)
        {
            if (numberCounts.ContainsKey(number))
            {
                numberCounts[number]++;
            }

            else
            {
                numberCounts[number] = 1;
            }
        }

        return numberCounts;
    }

    private static int CalculateTriples(Dictionary<int, int> numberCounts)
    {
        int score = 0;
        foreach (var numberCount in numberCounts)
        {
            if (numberCount.Value >= 3)
            {
                score += TripleScores[numberCount.Key];
                numberCounts[numberCount.Key] -= 3;
            }
        }

        return score;
    }

    private static int CalculateSingles(Dictionary<int, int> numberCounts)
    {
        int score = 0;

        foreach (var numberCount in numberCounts)
        {
            if (numberCount.Key == 1)
            {
                score += SingleOne * numberCounts[1];
            }

            if (numberCount.Key == 5)
            {
                score += SingleFive * numberCounts[5];
            }
        }

        return score;
    }
}
