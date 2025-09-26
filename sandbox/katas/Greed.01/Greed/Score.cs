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

        int score = 0;
        foreach (var numberCount in numberCounts)
        {
            if (numberCount.Value >= 3)
            {
                score += TripleScores[numberCount.Key];
                numberCounts[numberCount.Key] -= 3;
            }

            if (numberCount.Key == 1)
            {
                score += SingleOne * numberCounts[numberCount.Key];
            }

            if (numberCount.Key == 5)
            {
                score += SingleFive * numberCounts[numberCount.Key];
            }
        }

        return score;
    }
}
