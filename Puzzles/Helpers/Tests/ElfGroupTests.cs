namespace TestProject1.Helpers.Tests;

public class ElfGroupTests
{
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
}