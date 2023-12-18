using NUnit.Framework.Constraints;

namespace TestProject1.Helpers.Tests;

public class GalaxyMapShould
{
    private const string ExampleGalaxyMap = @"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....";

    [Test]
    public void Should_map_galaxies()
    {
        var galaxyMap = new GalaxyMap(PuzzleInput.InputStringToArray(ExampleGalaxyMap));
        Assert.Multiple(() =>
        {
            Assert.That(galaxyMap.GetItem(3, 0), Is.EqualTo("#"));
            Assert.That(galaxyMap.GetItem(0, 2), Is.EqualTo("#"));
            Assert.That(galaxyMap.GetItem(0, 9), Is.EqualTo("#"));
            Assert.That(galaxyMap.GetItem(7, 1), Is.EqualTo("#"));
        });
    }

    [TestCase(0,true)]
    [TestCase(3,false)]
    public void Should_identify_if_row_has_no_galaxies(int rowIndex, bool shouldHaveGalaxy)
    {
        var galaxyMap = new GalaxyMap(PuzzleInput.InputStringToArray(ExampleGalaxyMap));
        Assert.That(galaxyMap.RowHasGalaxy(rowIndex),Is.EqualTo(shouldHaveGalaxy));
    }   
}

public class GalaxyMap : PuzzleGrid
{
    public GalaxyMap(IEnumerable<string> mapLines) : base(mapLines)
    {
        
    }

    public bool RowHasGalaxy(int rowIndex)
    {
        return (GridCompass.AllItemsEast(rowIndex, 0).Contains("#"));
    }
}