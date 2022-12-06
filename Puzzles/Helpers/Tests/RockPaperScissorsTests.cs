namespace TestProject1.Helpers.Tests;

public class RockPaperScissorsTests
{
    [TestCase(RockPaperScissorsGameChoice.Rock,RockPaperScissorsGameChoice.Scissors)]
    [TestCase(RockPaperScissorsGameChoice.Paper,RockPaperScissorsGameChoice.Rock)]
    [TestCase(RockPaperScissorsGameChoice.Scissors,RockPaperScissorsGameChoice.Paper)]
    public void Should_determine_loss_score_is_zero(RockPaperScissorsGameChoice theyWin, RockPaperScissorsGameChoice weLose)
    {
        var result = RockPaperScissors.Result(theyWin, weLose);
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Should_determine_draw_score_is_three([Values] RockPaperScissorsGameChoice drawChoice)
    {
        var result = RockPaperScissors.Result(drawChoice, drawChoice);
        Assert.That(result, Is.EqualTo(3));
    }
        
    [TestCase(RockPaperScissorsGameChoice.Rock,RockPaperScissorsGameChoice.Scissors)]
    [TestCase(RockPaperScissorsGameChoice.Paper,RockPaperScissorsGameChoice.Rock)]
    [TestCase(RockPaperScissorsGameChoice.Scissors,RockPaperScissorsGameChoice.Paper)]
    public void Should_determine_win_score_is_six(RockPaperScissorsGameChoice weWin, RockPaperScissorsGameChoice theyLose)
    {
        var result = RockPaperScissors.Result(theyLose, weWin);
        Assert.That(result, Is.EqualTo(6));
    }

        
    [TestCase(RockPaperScissorsGameChoice.Rock,RockPaperScissorsGameChoice.Scissors)]
    [TestCase(RockPaperScissorsGameChoice.Paper,RockPaperScissorsGameChoice.Rock)]
    [TestCase(RockPaperScissorsGameChoice.Scissors,RockPaperScissorsGameChoice.Paper)]
    public void Round_score_total_for_win(RockPaperScissorsGameChoice weWin, RockPaperScissorsGameChoice theyLose)
    {
        var result = RockPaperScissors.Score(theyLose, weWin);
        var valueOfOurChoice = (int) weWin;
        Assert.That(result, Is.EqualTo(6 + valueOfOurChoice));
    }

    //public static Tuple<RockPaperScissorsGameChoice, RockPaperScissorsGameChoice> tuple1 = new {RockPaperScissorsGameChoice.Rock,RockPaperScissorsGameChoice.Paper}
    [TestCaseSource(nameof(PlayerChoices))]
    public void Should_convert_puzzle_input_to_game_choices(string input, Tuple<RockPaperScissorsGameChoice, RockPaperScissorsGameChoice> expected)
    {
        var choices = RockPaperScissors.PlayerChoices(input);
        Assert.That(choices,Is.EqualTo(expected));
    }

    [TestCase("A","A B")]
    [TestCase("B","B C")]
    [TestCase("C","C A")]
    public void Should_convert_second_input_into_winning_throw_when_is_z(string playerA, string expected)
    {
        var input = $"{playerA} Z";
        var winningInput = RockPaperScissors.FixResult(input);
        Assert.That(winningInput, Is.EqualTo(expected));
    }
    
    [TestCase("A","A A")]
    [TestCase("B","B B")]
    [TestCase("C","C C")]
    public void Should_convert_second_input_into_draw_throw_when_is_y(string playerA, string expected)
    {
        var input = $"{playerA} Y";
        var winningInput = RockPaperScissors.FixResult(input);
        Assert.That(winningInput, Is.EqualTo(expected));
    }
    
    [TestCase("A","A C")]
    [TestCase("B","B A")]
    [TestCase("C","C B")]
    public void Should_convert_second_input_into_losing_throw_when_is_x(string playerA, string expected)
    {
        var input = $"{playerA} X";
        var winningInput = RockPaperScissors.FixResult(input);
        Assert.That(winningInput, Is.EqualTo(expected));
    }

    public static IEnumerable<object[]> PlayerChoices()
    {
        yield return new object[] { "A X", new Tuple<RockPaperScissorsGameChoice, RockPaperScissorsGameChoice>( RockPaperScissorsGameChoice.Rock, RockPaperScissorsGameChoice.Rock ) };
        yield return new object[] { "B X", new Tuple<RockPaperScissorsGameChoice, RockPaperScissorsGameChoice>( RockPaperScissorsGameChoice.Paper, RockPaperScissorsGameChoice.Rock ) };
        yield return new object[] { "C X", new Tuple<RockPaperScissorsGameChoice, RockPaperScissorsGameChoice>( RockPaperScissorsGameChoice.Scissors, RockPaperScissorsGameChoice.Rock ) };
        yield return new object[] { "A Y", new Tuple<RockPaperScissorsGameChoice, RockPaperScissorsGameChoice>( RockPaperScissorsGameChoice.Rock, RockPaperScissorsGameChoice.Paper ) };
        yield return new object[] { "A Z", new Tuple<RockPaperScissorsGameChoice, RockPaperScissorsGameChoice>( RockPaperScissorsGameChoice.Rock, RockPaperScissorsGameChoice.Scissors ) };
    }
}