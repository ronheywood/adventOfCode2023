using System.Net.Sockets;
using NUnit.Framework.Constraints;

namespace TestProject1.Helpers;

public class ForestGrid
{
    public ForestGrid(IEnumerable<string> grid)
    {
        Trees = new List<int>();
        var rows = grid.Select(row => row.ToCharArray()).ToArray();
        GridHeight = rows.Length;
        foreach (var row in rows)
        {
            var trees = row.Select(i => int.Parse(char.ToString(i)));
            GridWidth = row.Length;
            Trees.AddRange(trees);
        }
    }

    public List<int> Trees { get; set; }

    public int GridWidth { get; private set; }

    public int GridHeight { get; private set; }

    public IEnumerable<int> VisibleTrees()
    {
        var visible = new List<int>();
        for (var x = 0; x < GridWidth; x++)
        {
            for (var y = 0; y < GridHeight; y++)
            {
                if (IsTreeVisible(x, y)) visible.Add(GetTree(x, y));
            }
        }

        return visible;
    }

    public bool IsTreeVisible(int x, int y)
    {
        var treeHeight = GetTree(x, y);

        var treesWestOfMe = TreesWest(x,y);
        var treesSouthOfMe = TreesSouth(x,y);
        var treesEastOfMe = TreesEast(x,y);
        
        var visibleFromTheNorth = VisibleNorth(x,y);
        var visibleFromTheWest = (treesWestOfMe.All(west => west < treeHeight));
        var visibleFromTheSouth = (treesSouthOfMe.All(south => south < treeHeight));
        var visibleFromTheEast = (treesEastOfMe.All(east => east < treeHeight));

        return visibleFromTheNorth || visibleFromTheWest || visibleFromTheSouth || visibleFromTheEast;
    }

    public int GetTree(int x, int y)
    {
        var column = y * GridWidth;
        var rowStart = Trees.Skip(column);
        return rowStart.Skip(x).First();
    }

    public IEnumerable<int> TreesNorth(int x, int y)
    {
        if (y == 0) return Enumerable.Empty<int>();

        var result = new List<int>();

        for (var row = y; row > 0; row--)
        {
            result.Add(GetTree(x, row - 1));
        }

        return result;
    }

    public IEnumerable<int> TreesSouth(int x, int y)
    {
        if (y >= GridHeight - 1)
            return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var row = y; row < GridHeight - 1; row++)
        {
            result.Add(GetTree(x, row + 1));
        }

        return result;
    }

    public IEnumerable<int> TreesWest(int x, int y)
    {
        if (x == 0) return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var column = x; column > 0; column--)
        {
            result.Add(GetTree(column-1, y));
        }

        result.Reverse();
        return result;
    }

    public IEnumerable<int> TreesEast(int x, int y)
    {
        if (x == GridWidth - 1)
            return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var column = x; column < GridWidth-1; column++)
            result.Add(GetTree(column + 1, y));
        return result;
    }

    public bool VisibleNorth(int x, int y)
    {
        var treeHeight = GetTree(x, y);

        var treesNorthOfMe = TreesNorth(x,y);
        
        return (treesNorthOfMe.All(north => north < treeHeight));
    }

    public bool VisibleSouth(int x, int y)
    {
        var treeHeight = GetTree(x, y);
        var treesSouthOfMe = TreesSouth(x,y);
        return (treesSouthOfMe.All(south => south < treeHeight));
    }
    
    public bool VisibleEast(int x, int y)
    {
        var treeHeight = GetTree(x, y);
        var treesEastOfMe = TreesEast(x,y);
        return (treesEastOfMe.All(east => east < treeHeight));
    }
    
    public bool VisibleWest(int x, int y)
    {
        var treeHeight = GetTree(x, y);
        var treesWestOfMe = TreesWest(x,y);
        return (treesWestOfMe.All(west => west < treeHeight));
    }
    
}