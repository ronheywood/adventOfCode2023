using TestProject1.Helpers;
using TestProject1.Helpers.Tests;

namespace TestProject1;

public class Day3
{
    [Test]
    public void SumThePrioritiesOfTheContents()
    {
        var contents = new []
        {
            "vJrwpWtwJgWrhcsFMMfFFhFp",
            "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
            "PmmdzqPrVvPwwTWBwg",
            "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
            "ttgJtRGJQctTZtZT",
            "CrZsJsPPZsGzwwsLwLmpwMDw"
        };
        var sum = 0;
        foreach(var content in contents)
        {
            sum += RuckSack.GetPriorty(content);
        }
        Assert.That(sum, Is.EqualTo(157));
    }
    
    [TestCase(7831)]
    public void SumThePrioritiesOfThePuzzleInput(int correctAnswer)
    {
        var contents = PuzzleInput.GetFile("day3.txt");
        var sum = 0;
        foreach(var content in contents)
        {
            sum += RuckSack.GetPriorty(content);
        }
        Assert.That(sum, Is.EqualTo(correctAnswer));
    }
    
    [Test]
    public void SumThePrioritiesOfBadges()
    {
        var contents = new []
        {
            "vJrwpWtwJgWrhcsFMMfFFhFp",
            "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
            "PmmdzqPrVvPwwTWBwg",
            "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
            "ttgJtRGJQctTZtZT",
            "CrZsJsPPZsGzwwsLwLmpwMDw"
        };
        var elfGroups = ElfGroup.FromInventory(contents);
        var sum = 0;
        foreach(var group in elfGroups)
        {
            var badge = ElfGroup.BadgeCode(group); 
            sum += RuckSack.Priority(badge);
        }
        Assert.That(sum, Is.EqualTo(70));
    }
    
    [TestCase(2683)]
    public void SumThePrioritiesOfBadgesFromPuzzleInput(int correctAnswer)
    {
        var contents = PuzzleInput.GetFile("day3.txt");
        var elfGroups = ElfGroup.FromInventory(contents);
        var sum = 0;
        foreach(var group in elfGroups)
        {
            var badge = ElfGroup.BadgeCode(group); 
            sum += RuckSack.Priority(badge);
        }
        Assert.That(sum, Is.EqualTo(correctAnswer));
    }
}