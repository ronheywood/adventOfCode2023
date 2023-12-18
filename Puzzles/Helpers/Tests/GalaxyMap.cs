using NUnit.Framework.Constraints;

namespace TestProject1.Helpers.Tests;

public class GalaxyMap : PuzzleGrid
{
    public GalaxyMap(IEnumerable<string> mapLines) : base(mapLines)
    {
        
    }

    public bool RowHasGalaxy(int rowIndex)
    {
        return (GridCompass.AllItemsEast(rowIndex, 0).Contains("#"));
    }

    public bool ColumnHasGalaxy(int columnIndex)
    {
        return (GridCompass.AllItemsSouth(columnIndex, 0).Contains("#"));
    }

    public int Distance(Tuple<int, int> location1, Tuple<int, int> location2)
    {
        if (location1.Item1 == location2.Item1)
        {
            //moving along Y;
            return location2.Item2 - location1.Item2;
        }
        //moving along x;
        return location2.Item1 - location1.Item1;
    }
}