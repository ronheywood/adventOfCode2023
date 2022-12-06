using TestProject1.Helpers;

namespace TestProject1;

public class Day5
{
    [TestCase(@"BWNCQRMDB")]
    public void Should_process_all_instructions_from_puzzle_input(string expected)
    {
        var input = PuzzleInput.GetFile("day5.txt");
        var crane = new CraneStack(input);
        crane.ProcessAllInstructions();
        var answerString = crane.GetTopCrates();
        Assert.That(answerString,Is.EqualTo(expected));
    }
    
    [TestCase(@"NHWZCBNBF")]
    public void Should_process_all_instructions_from_puzzle_input_with_upgraded_move(string expected)
    {
        var input = PuzzleInput.GetFile("day5.txt");
        var crane = new CraneStack(input);
        crane.CanMoveMany = true;
        crane.ProcessAllInstructions();
        var answerString = crane.GetTopCrates();
        Assert.That(answerString,Is.EqualTo(expected));
    }
}