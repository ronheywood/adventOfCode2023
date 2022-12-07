using TestProject1.Helpers;

namespace TestProject1;

public class Day7
{
    [Test]
    [Ignore("There are duplicates")]
    public void Dir_names_are_duplicated()
    {
        var inputLines = PuzzleInput.GetFile("day7.txt");
        var dirOutput = inputLines.Where(l => l.StartsWith("dir ")).ToArray();
        Assert.That(dirOutput.Count(), Is.EqualTo(dirOutput.Distinct().Count()));
    }
    
    [TestCase(1915606)]
    //1409446 is too low because there are duplicates in the directory names
    public void Should_get_total_size_from_puzzle_input(int correctAnswer)
    {
        var inputLines = PuzzleInput.GetFile("day7.txt");
        var c = new CommunicatorFileSystem();
        c.ProcessInput(inputLines);
        var smallDirectories = c.DirectoriesWithTotalSizeUpTo(100000);
        Assert.That(smallDirectories.Sum(d => c.DirectoryFileSize(d.Name)), Is.EqualTo(correctAnswer));
    }
}