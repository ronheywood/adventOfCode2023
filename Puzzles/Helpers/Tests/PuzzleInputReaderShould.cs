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

    [Test]
    public void Convert_a_newline_separated_string_to_array()
    {
        string testString = @"Line 1
Line 2
Line 3";

        IEnumerable actualLineContent = PuzzleInput.InputStringToArray(testString);
        IEnumerable expectedLineContent = new []{"Line 1","Line 2","Line 3"};
        CollectionAssert.AreEquivalent(expectedLineContent,actualLineContent);
    }

    [Test]
    public void Convert_puzzle_lines_to_multi_dimensional_array()
    {
        //var exampleInput = 
    }

    [Test]
    public void Collate_puzzle_sections_into_smaller_groups()
    {
        var exampleInput = PuzzleInput.InputStringToArray(@"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000");
        var segments = PuzzleInput.GetPuzzleSegments(exampleInput, "").ToArray();
        Assert.That(segments.Count,Is.EqualTo(5));
        CollectionAssert.AreEquivalent(segments[0],new[] {"1000","2000","3000"});
        CollectionAssert.AreEquivalent(segments[1],new[] {"4000"});
        CollectionAssert.AreEquivalent(segments[2],new[] {"5000","6000"});
        CollectionAssert.AreEquivalent(segments[3],new[] {"7000","8000","9000"});
        CollectionAssert.AreEquivalent(segments[4],new[] {"10000"});
    }

    [Test]
    public void Collate_puzzle_input_as_a_tuple_of_strings()
    {
        var exampleInput = PuzzleInput.InputStringToArray(@"A Y
B X
C Z");
        var pairs = PuzzleInput.GetPuzzlePairs(exampleInput, " ").ToArray();
        Assert.That(pairs.Count,Is.EqualTo(3));
        Assert.That(pairs[0],Is.EqualTo(new Tuple<string,string>("A","Y")));
        Assert.That(pairs[1],Is.EqualTo(new Tuple<string,string>("B","X")));
        Assert.That(pairs[2],Is.EqualTo(new Tuple<string,string>("C","Z")));
    }

    [Test]
    public void Collate_puzzle_input_as_a_tuple_of_strings_split_in_half()
    {
        var exampleInput = PuzzleInput.InputStringToArray(@"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw");
        var answer = PuzzleInput.GetPuzzleLinesSplit(exampleInput);
        Assert.That(answer.Count(), Is.EqualTo(6));
    }
}