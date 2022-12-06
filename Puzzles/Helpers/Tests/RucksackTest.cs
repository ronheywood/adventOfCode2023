using NUnit.Framework.Constraints;

namespace TestProject1.Helpers.Tests;

public class RucksackTest
{
    [Test]
    public void Should_expect_an_even_number_of_alphanumeric_item_codes()
    {
        var contents = "a";
        var ex = Assert.Throws<Exception>(() => RuckSack.Inventory(contents));
        Assert.That(ex.Message, Is.EqualTo("Uneven number of items"));
        
        Assert.DoesNotThrow(() => RuckSack.Inventory("aa"));
        
    }
    
    [Test]
    public void Should_expect_items_are_only_letters()
    {
        var contents = "11";
        var ex = Assert.Throws<Exception>(() => RuckSack.Inventory(contents));
        
        var contents2 = "{{";
        var ex2 = Assert.Throws<Exception>(() => RuckSack.Inventory(contents));
        
        var contents3 = "[[";
        var ex3 = Assert.Throws<Exception>(() => RuckSack.Inventory(contents));
        Assert.Multiple(() =>
        {
            Assert.That(ex.Message, Is.EqualTo("Unexpected item code"));
            Assert.That(ex2.Message, Is.EqualTo("Unexpected item code"));
            Assert.That(ex3.Message, Is.EqualTo("Unexpected item code"));
        });
    }

    
    [TestCaseSource(nameof(CollectionArray))]
    public void Should_split_letters_into_two(string contents, IEnumerable<char[]> expectedCollection)
    {
        var contentsInCompartments = RuckSack.Inventory(contents).ToArray();
        Assert.That(contentsInCompartments.Count(),Is.EqualTo(2));
        CollectionAssert.AreEquivalent(expectedCollection,contentsInCompartments);
    }

    [TestCaseSource(nameof(DuplicateContents))]
    public void Identifies_common_items_in_both_compartments(char[] compartmentOneContents, char[] compartmentTwoContents, char expectedDuplicate)
    {
        var commonChar = RuckSack.FindDuplicate(compartmentOneContents, compartmentTwoContents); 
        Assert.That(commonChar, Is.EqualTo(expectedDuplicate));
    }

    [Test]
    public void Should_convert_char_to_priority()
    {
        Assume.That(('a' - 0), Is.EqualTo(97));
        Assume.That(('z' - 0), Is.EqualTo(122));
        Assume.That(('A' - 0), Is.EqualTo(65));
        Assume.That(('Z' - 0), Is.EqualTo(90));
        
        Assert.That(RuckSack.Priority('a'), Is.EqualTo(1));
        Assert.That(RuckSack.Priority('z'), Is.EqualTo(26));
        Assert.That(RuckSack.Priority('A'), Is.EqualTo(27));
        Assert.That(RuckSack.Priority('Z'), Is.EqualTo(52));
    }
    
    [Test]
    public void FindThePriorityOfDuplicateItemsinARucksack()
    {
        var input1 = "vJrwpWtwJgWrhcsFMMfFFhFp";
        var epxecteDuplicateChar = 'p';
        var epxectedValue = 16;
        var sum = RuckSack.GetPriorty(input1);
        Assert.That(sum, Is.EqualTo(epxectedValue));
    }

    [Test]
    public void Elf_packs_have_three_rucksacks_per_group()
    {
        var expectedGroup1 = new[]
        {
            "vJrwpWtwJgWrhcsFMMfFFhFp",
            "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
            "PmmdzqPrVvPwwTWBwg"
        };
        var expectedGroup2 = new[]
        {
            "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
            "ttgJtRGJQctTZtZT",
            "CrZsJsPPZsGzwwsLwLmpwMDw"
        };
        var expectedGroup3 = new[]
        {
            "Aa",
            "Ba",
            "Ca"
        };
        var ruckSackCollection = expectedGroup1.Concat(expectedGroup2).Concat(expectedGroup3);
        var elfPacks = ElfGroup.FromInventory(ruckSackCollection).ToArray();
        Assert.That(elfPacks.Count(), Is.EqualTo(3));
        CollectionAssert.AreEquivalent(elfPacks[0], expectedGroup1);
        CollectionAssert.AreEquivalent(elfPacks[1], expectedGroup2);
        CollectionAssert.AreEquivalent(elfPacks[2], expectedGroup3);
    }

    [TestCaseSource(nameof(ElfGroups))]
    public void Should_identify_common_item_in_a_group_as_a_badge_code(IEnumerable<string> group, char expected)
    {
        Assert.That(ElfGroup.BadgeCode(group), Is.EqualTo(expected));
    }

    private static IEnumerable<object> ElfGroups()
    {
        yield return new object[]
        {
            new[]
            {
                "vJrwpWtwJgWrhcsFMMfFFhFp",
                "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
                "PmmdzqPrVvPwwTWBwg"
            },
            'r'
        };
        yield return new object[]
        {
            new[]
            {
                "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
                "ttgJtRGJQctTZtZT",
                "CrZsJsPPZsGzwwsLwLmpwMDw"
            },
            'Z'
        };
    }

    private static IEnumerable<object[]> DuplicateContents()
    {
        yield return new object[] { new[] { 'A', 'b', 'c' }, new[] { 'A', 'b', 'c' }, 'A' };
        yield return new object[] { new[] { 'A', 'b', 'c' }, new[] { 'A', 'b', 'c' }, 'A' };
        yield return new object[] { new[] { 'A', 'b', 'c' }, new[] { 'X', 'Y', 'Z' }, null };
        yield return new object[] { "vJrwpWtwJgWr".ToCharArray(), "hcsFMMfFFhFp".ToCharArray(), 'p' };
        yield return new object[] { "jqHRNqRjqzjGDLGL".ToCharArray(), "rsFMfFZSrLrFZsSL".ToCharArray(), 'L' };
        yield return new object[] { "PmmdzqPrV".ToCharArray(), "vPwwTWBwg".ToCharArray(), 'P' };
        yield return new object[] { "wMqvLMZHhHMvwLH".ToCharArray(), "jbvcjnnSBnvTQFn".ToCharArray(), 'v' };
        yield return new object[] { "ttgJtRGJ".ToCharArray(), "QctTZtZT".ToCharArray(), 't' };
        yield return new object[] { "CrZsJsPPZsGz".ToCharArray(), "wwsLwLmpwMDw".ToCharArray(), 's' };
    }

    private static IEnumerable<object[]> CollectionArray()
    {
        yield return new object[] { "AA", new List<char[]> { "A".ToCharArray(), "A".ToCharArray() } };
        yield return new object[] { "AaBb", new List<char[]> { "Aa".ToCharArray(), "Bb".ToCharArray() } };
        yield return new object[] { "aZbFhBbQ", new List<char[]> { "aZbF".ToCharArray(), "hBbQ".ToCharArray() } };
    }
}