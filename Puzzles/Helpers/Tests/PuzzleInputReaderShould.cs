using System.Collections;
using Microsoft.VisualBasic.FileIO;

namespace TestProject1.Helpers.Tests;

public class PuzzleInputReaderShould
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetInputForPuzzle()
    {
        IEnumerable<string> expectedLineContent = new[] { "line 1", "line 2", "", "line 3" };
        var testFile = Path.Combine(PuzzleInput.BaseDirectory,"test");
        using (var fp = FileSystem.OpenTextFileWriter(testFile,false))
        {
            foreach (var line in expectedLineContent)
            {
                fp.WriteLine(line);
            }
            fp.Close();
        }

        IEnumerable actualLineContent = PuzzleInput.GetFile("test");
        CollectionAssert.AreEquivalent(expectedLineContent,actualLineContent);
    }
}