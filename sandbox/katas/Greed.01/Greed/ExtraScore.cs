namespace Greed;

public class ExtraScore : Score
{
    public int CalculateExtraScore(List<int> numbers)
    {
        if (numbers == null || numbers.Count == 0)
        {
            return 0;
        }

        var numberCounts = CountNumbers(numbers);

        if (numbers.Count == 6)
        {
            if (CheckThreePairs(numberCounts))
            {
                return 800;
            }

            if (CheckStraight(numberCounts))
            {
                return 1200;
            }
        }

        int score = 0;
        score += CalculateExtraMultiples(numberCounts);
        score += CalculateSingles(numberCounts);

        return score;
    }

    private int CalculateExtraMultiples(Dictionary<int, int> numberCounts)
    {
        int score = 0;
        int[] multipliers = { 0, 0, 0, 1, 2, 4, 8 };

        foreach (var numberCount in numberCounts)
        {
            if (numberCount.Value >= 3)
            {
                int multiplier = multipliers[numberCount.Value];
                score += multiplier * ScoreRules.TripleScores[numberCount.Key];
                numberCounts[numberCount.Key] -= numberCount.Value;
            }
        }

        return score;
    }


    private bool CheckThreePairs(Dictionary<int, int> counts)
    {
        return counts.Values.Count(v => v == 2) == 3;
    }

    private bool CheckStraight(Dictionary<int, int> counts)
    {
        return counts.Count == 6 && counts.Values.All(v => v == 1);
    }
}
