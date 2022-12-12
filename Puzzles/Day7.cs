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

    [TestCase(5025657)]
    //27760227 is too high
    //25725669 is also too high
    public void Should_get_smallest_directory_to_delete(int correctAnswer)
    {
        var inputLines = PuzzleInput.GetFile("day7.txt");
        var c = new CommunicatorFileSystem();
        c.ProcessInput(inputLines);
        var targetDirectoriesToFreeSpace = c.GetDirectoryToDelete();

        var answer = c.DirectoryFileSize(targetDirectoriesToFreeSpace.First().Name);
        Assert.That(answer, Is.EqualTo(correctAnswer));
    }
}