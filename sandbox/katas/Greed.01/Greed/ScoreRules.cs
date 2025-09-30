namespace Greed;

public static class ScoreRules
{
    public static readonly Dictionary<int, int> TripleScores = new Dictionary<int, int>
    {
        {1, 1000},
        {2, 200},
        {3, 300},
        {4, 400},
        {5, 500},
        {6, 600}
    };

    public const int SingleOne = 100;
    public const int SingleFive = 50;
}
