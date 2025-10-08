using Greed;

var baseNumbers = new List<int> { 1, 1, 1, 5, 1 };

var score = new Score();
int result = score.CalculateBaseScore(baseNumbers);
Console.WriteLine("Base Score: " + result);

var extraNumbers = new List<int> { 1, 3, 2, 4, 5, 6 };
var extraScore = new ExtraScore();
int extraResult = extraScore.CalculateExtraScore(baseNumbers);
Console.WriteLine("Extra score: " + extraResult);
Console.WriteLine("Extra score: " + extraScore.CalculateExtraScore(extraNumbers));
