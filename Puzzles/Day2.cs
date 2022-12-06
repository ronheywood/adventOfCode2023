using NUnit.Framework.Constraints;
using TestProject1.Helpers;

namespace TestProject1;

public class Day2
{
    [TestCaseSource(nameof(Scores))]
    public void Sum_scores_simple(string[] input, int expected)
    {
        Assert.That(RockPaperScissors.GameResult(input), Is.EqualTo(expected));
    }
    
    [TestCaseSource(nameof(RiggedScores))]
    public void Sum_scores_rigged(string[] input, int expected)
    {
        Assert.That(RockPaperScissors.GameResult(input,true), Is.EqualTo(expected));
    }
    
    [TestCase(11666)]
    public void Correct_answer_from_input_should_be(int correctAnswer)
    {
        var input = PuzzleInput.GetFile("day2.txt");
        Assert.That(input, Is.Not.Empty);
        Assert.That(RockPaperScissors.GameResult(input), Is.EqualTo(correctAnswer));
    }
    
    [TestCase(12767)]
    public void Correct_answer_from_rigged_input_should_be(int correctAnswer)
    {
        var input = PuzzleInput.GetFile("day2.txt");
        Assert.That(input, Is.Not.Empty);
        Assert.That(RockPaperScissors.GameResult(input,true), Is.EqualTo(correctAnswer));
    }

    private static IEnumerable<object[]> Scores()
    {
        yield return new object[] { new[] {"A A"}, 4 };
        yield return new object[] { new[] {"A Y","B X", "C Z"}, 15 };
    }
    
    private static IEnumerable<object[]> RiggedScores()
    {
        yield return new object[] { new[] {"A X"}, 0 + RockPaperScissorsGameChoice.Scissors };
        yield return new object[] { new[] {"A Y"}, 3 + RockPaperScissorsGameChoice.Rock};
        yield return new object[] { new[] {"A Z"}, 6 + RockPaperScissorsGameChoice.Paper };
        
    }
}