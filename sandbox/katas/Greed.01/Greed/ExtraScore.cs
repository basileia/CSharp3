namespace Greed;

public class ExtraScore : Score
{
    public int CalculateExtraScore(List<int> numbers)
    {
        var numberCounts = CountNumbers(numbers);

        if (numbers.Count == 6)
        {
            if (CheckThreePairs(numberCounts))
            {
                return 800;
            }

            if (CheckStraight(numbers))
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

        foreach (var numberCount in numberCounts)
        {
            if (numberCount.Value >= 3)
            {
                int multiplier = (int)Math.Pow(2, numberCount.Value - 3);
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

    private bool CheckStraight(List<int> numbers)
    {
        var sorted = numbers.OrderBy(x => x).ToList();
        return sorted.SequenceEqual(new List<int> { 1, 2, 3, 4, 5, 6 });
    }
}
