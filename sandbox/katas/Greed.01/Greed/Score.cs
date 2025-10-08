namespace Greed;

public class Score
{
    public int CalculateBaseScore(List<int> numbers)
    {
        if (numbers == null || numbers.Count == 0)
        {
            return 0;
        }

        var numberCounts = CountNumbers(numbers);
        int score = 0;
        score += CalculateTriples(numberCounts);
        score += CalculateSingles(numberCounts);

        return score;
    }

    protected Dictionary<int, int> CountNumbers(List<int> numbers)
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

    protected int CalculateTriples(Dictionary<int, int> numberCounts)
    {
        int score = 0;
        foreach (var numberCount in numberCounts)
        {
            if (numberCount.Value >= 3)
            {
                score += ScoreRules.TripleScores[numberCount.Key];
                numberCounts[numberCount.Key] -= 3;
            }
        }

        return score;
    }

    protected int CalculateSingles(Dictionary<int, int> numberCounts)
    {
        int score = 0;

        foreach (var numberCount in numberCounts)
        {
            if (numberCount.Key == 1)
            {
                score += ScoreRules.SingleOne * numberCounts[1];
            }

            if (numberCount.Key == 5)
            {
                score += ScoreRules.SingleFive * numberCounts[5];
            }
        }

        return score;
    }
}
