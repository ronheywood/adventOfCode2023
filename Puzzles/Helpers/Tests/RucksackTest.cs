// ReSharper disable StringLiteralTypo
namespace TestProject1.Helpers.Tests;

public class RucksackTest
{
    [Test]
    public void Should_expect_an_even_number_of_alphanumeric_item_codes()
    {
        var contents = "a";
        var ex = Assert.Throws<Exception>(() => RuckSack.Inventory(contents));
        Assert.That(ex?.Message, Is.EqualTo("Uneven number of items"));
        
        Assert.DoesNotThrow(() => RuckSack.Inventory("aa"));
        
    }
    
    [Test]
    public void Should_expect_items_are_only_letters()
    {
        var contents = "11";
        var ex = Assert.Throws<Exception>(() => RuckSack.Inventory(contents));
        
        var contents2 = "{{";
        var ex2 = Assert.Throws<Exception>(() => RuckSack.Inventory(contents2));
        
        var contents3 = "[[";
        var ex3 = Assert.Throws<Exception>(() => RuckSack.Inventory(contents3));
        Assert.Multiple(() =>
        {
            Assert.That(ex?.Message, Is.EqualTo("Unexpected item code"));
            Assert.That(ex2?.Message, Is.EqualTo("Unexpected item code"));
            Assert.That(ex3?.Message, Is.EqualTo("Unexpected item code"));
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
    public void FindThePriorityOfDuplicateItemsInARucksack()
    {
        const string contents = "vJrwpWtwJgWrhcsFMMfFFhFp";
        var expected = RuckSack.Priority('p');
        var sum = RuckSack.GetPriorty(contents);
        Assert.That(sum, Is.EqualTo(expected));
    }

    private static IEnumerable<object?[]> DuplicateContents()
    {
        yield return new object[] { new[] { 'A', 'b', 'c' }, new[] { 'A', 'b', 'c' }, 'A' };
        yield return new object[] { new[] { 'A', 'b', 'c' }, new[] { 'A', 'b', 'c' }, 'A' };
        yield return new object?[] { new[] { 'A', 'b', 'c' }, new[] { 'X', 'Y', 'Z' }, null };
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