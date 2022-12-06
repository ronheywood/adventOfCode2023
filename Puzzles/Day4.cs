using TestProject1.Helpers;
using TestProject1.Helpers.Tests;

namespace TestProject1;

public class Day4
{
    [Test]
    public void Sum_of_pairs_with_invalid_sections()
    {
        var input = new[]
        {
            ".234.....",
            ".....678.",
            "",
            ".23......",
            "...45....",
            "",
            "....567..",
            "......789",
            "",
            ".2345678.",
            "..34567..",
            "",
            ".....6...",
            "...456...",
            "",
            ".23456...",
            "...45678."
        };
        
        var sectionPairs = CleanupSection.Sections(input);
        var invalidSum = 0;
        foreach (var section in sectionPairs)
        {
            invalidSum += CleanupSection.Validate(section.Item1.ToArray(),section.Item2.ToArray()) ? 0 : 1;
        }

        Assert.That(invalidSum, Is.EqualTo(2));
    }
    
    [TestCase(524)]
    public void Sum_of_pairs_with_invalid_sections_from_puzzle_input(int correctAnswer)
    {
        var input = PuzzleInput.GetFile("day4.txt");
        var sectionPairs = new List<Tuple<IEnumerable<int>, IEnumerable<int>>>();
        foreach (var line in input)
        {
            var dashSections = line.Split(",");
            var sector1 = CleanupSection.RangeToSector(dashSections[0]);
            var sector2 = CleanupSection.RangeToSector(dashSections[1]);
            sectionPairs.Add(new Tuple<IEnumerable<int>, IEnumerable<int>>(sector1, sector2));
        }
        
        var invalidSum = 0;
        foreach (var section in sectionPairs)
        {
            invalidSum += CleanupSection.Validate(section.Item1.ToArray(),section.Item2.ToArray()) ? 0 : 1;
        }

        Assert.That(invalidSum, Is.EqualTo(correctAnswer));
    }
}