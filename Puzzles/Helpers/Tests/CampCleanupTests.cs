using System.Collections;
using NUnit.Framework.Constraints;

namespace TestProject1.Helpers.Tests;

public class CampCleanupTests
{
    [Test]
    public void Should_convert_dash_seperated_ranges_to_dot_notation()
    {
        var range = "3-7";
        var result = CleanupSection.RangeToSector(range);
        CollectionAssert.AreEquivalent(new []{3,4,5,6,7},result);
    }

    [TestCaseSource(nameof(CampSections))]
    public void Should_extract_sections_in_pairs(string section1,int[] expectedSection1,string section2,int[] expectedSection2, bool _)
    {
        var section = CleanupSection.Sectors(section1, section2);
        CollectionAssert.AreEquivalent(expectedSection1, section.Item1);
        CollectionAssert.AreEquivalent(expectedSection2, section.Item2);
    }
    
    [TestCaseSource(nameof(CampSections))]
    public void Should_identify_if_one_pair_is_only_working_in_sections_covered_by_another_pair(string section1,int[] expectedSection1,string section2,int[] expectedSection2, bool valid)
    {
        var section = CleanupSection.Sectors(section1, section2);
        Assert.That(CleanupSection.Validate(section.Item1.ToArray(),section.Item2.ToArray()),Is.EqualTo(valid));
    }

    private static IEnumerable<object[]> CampSections()
    {
        yield return new object[] { ".234.....", new []{2,3,4}, ".....678.",  new[]{6,7,8}, true};
        yield return new object[] { ".234.....", new []{2,3,4}, ".....678.",  new[]{6,7,8}, true};
        yield return new object[] { ".23......", new []{2,3}, "...45....",  new[]{4,5}, true};
        yield return new object[] { "....567..", new []{5,6,7}, "......789",  new[]{7,8,9}, true};
        yield return new object[] { ".2345678.", new []{2,3,4,5,6,7,8}, "..34567..",  new[]{3,4,5,6,7}, false};
        yield return new object[] { ".....6...", new []{6}, "...456...",  new[]{4,5,6}, false};
        yield return new object[] { ".23456...", new []{2,3,4,5,6}, "...45678.",  new[]{4,5,6,7,8}, true};
    }
}