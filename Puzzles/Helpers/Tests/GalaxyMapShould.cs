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
    [TestCase(7,false)]
    public void Should_identify_if_row_has_galaxies(int rowIndex, bool shouldHaveGalaxy)
    {
        var galaxyMap = new GalaxyMap(PuzzleInput.InputStringToArray(ExampleGalaxyMap));
        Assert.That(galaxyMap.RowHasGalaxy(rowIndex),Is.EqualTo(shouldHaveGalaxy));
    }
    
    [TestCase(0,true)]
    [TestCase(2,false)]
    [TestCase(5,false)]
    [TestCase(8,false)]
    public void Should_identify_if_column_has_galaxies(int columnIndex, bool shouldHaveGalaxy)
    {
        var galaxyMap = new GalaxyMap(PuzzleInput.InputStringToArray(ExampleGalaxyMap));
        Assert.That(galaxyMap.ColumnHasGalaxy(columnIndex),Is.EqualTo(shouldHaveGalaxy));
    }

    [TestCase(0,0,1,0,1)]
    [TestCase(0,0,0,1,1)]
    [TestCase(0,0,0,2,2)]
    [TestCase(0,0,2,0,2)]
    [TestCase(0,4,6,4,6)]
    public void Should_identify_distance_between_two_aligned_locations_unaffected_by_expansion(
        int startX,
        int startY,
        int endX,
        int endY,
        int distance
        )
    {
        var galaxyMap = new GalaxyMap(PuzzleInput.InputStringToArray(ExampleGalaxyMap));
        var location1 = new Tuple<int, int>(startX,startY);
        var location2 = new Tuple<int, int>(endX,endY);
        Assert.That(galaxyMap.Distance(location1,location2),Is.EqualTo(distance));
    }
    
    
    // [TestCase(0,4,6,4,6)]
    // public void Should_identify_distance_between_two_aligned_locations_affected_by_expansion(
    //     int startX,
    //     int startY,
    //     int endX,
    //     int endY,
    //     int distance
    // )
    // {
    //     var galaxyMap = new GalaxyMap(PuzzleInput.InputStringToArray(ExampleGalaxyMap));
    //     var location1 = new Tuple<int, int>(startX,startY);
    //     var location2 = new Tuple<int, int>(endX,endY);
    //     Assert.That(galaxyMap.Distance(location1,location2),Is.EqualTo(distance));
    // }
}