using TestProject1.Helpers;

namespace TestProject1;

public class Day9
{
    [TestCase(5960)]
    public void Should_test(int correctAnswer)
    {
        var puzzleInput = PuzzleInput.GetFile("day9.txt");
        var rope = new Rope();
        rope.ProcessPuzzleInput(puzzleInput);
        Assert.That(rope.UniqueRecordedTailMovements, Has.Count.EqualTo(correctAnswer));

    }
}