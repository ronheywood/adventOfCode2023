using NUnit.Framework.Constraints;

namespace TestProject1.Helpers;

public class GridCompass
{
    private readonly ForestGrid _forestGrid;

    public GridCompass(ForestGrid forestGrid)
    {
        _forestGrid = forestGrid;
    }

    public IEnumerable<int> TreesNorth(int x, int y)
    {
        if (y == 0) return Enumerable.Empty<int>();

        var result = new List<int>();

        for (var row = y; row > 0; row--)
        {
            result.Add(_forestGrid.GetTree(x, row - 1));
        }

        return result;
    }

    public IEnumerable<int> TreesSouth(int x, int y)
    {
        if (y >= _forestGrid.GridHeight - 1)
            return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var row = y; row < _forestGrid.GridHeight - 1; row++)
        {
            result.Add(_forestGrid.GetTree(x, row + 1));
        }

        return result;
    }

    public IEnumerable<int> TreesWest(int x, int y)
    {
        if (x == 0) return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var column = x; column > 0; column--)
        {
            result.Add(_forestGrid.GetTree(column - 1, y));
        }
        return result;
    }

    public IEnumerable<int> TreesEast(int x, int y)
    {
        if (x == _forestGrid.GridWidth - 1)
            return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var column = x; column < _forestGrid.GridWidth - 1; column++)
            result.Add(_forestGrid.GetTree(column + 1, y));
        return result;
    }
}

public class ForestGrid
{
    private readonly GridCompass _gridCompass;

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

        _gridCompass = new GridCompass(this);
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
        var visibleFromTheNorth = VisibleNorth(x, y);
        var visibleFromTheWest = VisibleWest(x, y);
        var visibleFromTheSouth = VisibleSouth(x, y);
        var visibleFromTheEast = VisibleEast(x, y);

        return visibleFromTheNorth || visibleFromTheWest || visibleFromTheSouth || visibleFromTheEast;
    }

    public int GetTree(int x, int y)
    {
        var column = y * GridWidth;
        var rowStart = Trees.Skip(column);
        return rowStart.Skip(x).First();
    }

    private bool VisibleNorth(int x, int y)
    {
        var treeHeight = GetTree(x, y);

        var treesNorthOfMe = _gridCompass.TreesNorth(x, y);

        return (treesNorthOfMe.All(north => north < treeHeight));
    }

    private bool VisibleSouth(int x, int y)
    {
        var treeHeight = GetTree(x, y);
        var treesSouthOfMe = _gridCompass.TreesSouth(x, y);
        return (treesSouthOfMe.All(south => south < treeHeight));
    }

    private bool VisibleEast(int x, int y)
    {
        var treeHeight = GetTree(x, y);
        var treesEastOfMe = _gridCompass.TreesEast(x, y);
        return (treesEastOfMe.All(east => east < treeHeight));
    }

    private bool VisibleWest(int x, int y)
    {
        var treeHeight = GetTree(x, y);
        var treesWestOfMe = _gridCompass.TreesWest(x, y);
        return (treesWestOfMe.All(west => west < treeHeight));
    }

    public int ViewingDistanceNorth(int x, int y)
    {
        var treeHeight = GetTree(x, y);
        var treesInDirectionOfView = _gridCompass.TreesNorth(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }
    public int ViewingDistanceSouth(int x, int y)
    {
        var treeHeight = GetTree(x, y);
        var treesInDirectionOfView = _gridCompass.TreesSouth(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }

    public int ViewingDistanceEast(int x, int y)
    {
        var treeHeight = GetTree(x, y);
        var treesInDirectionOfView = _gridCompass.TreesEast(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }

    public int ViewingDistanceWest(int x, int y)
    {
        var treeHeight = GetTree(x, y);
        var treesInDirectionOfView = _gridCompass.TreesWest(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }
    

    public int ScenicScore(int x, int y)
    {
        
        var treeHeight = GetTree(x, y);
        
        var north = CountViewingDistance(_gridCompass.TreesNorth(x, y).ToArray(), treeHeight);
        var west = CountViewingDistance(_gridCompass.TreesWest(x, y).ToArray(), treeHeight);
        var east = CountViewingDistance(_gridCompass.TreesEast(x, y).ToArray(), treeHeight);
        var south = CountViewingDistance(_gridCompass.TreesSouth(x, y).ToArray(), treeHeight);

        return north * west * east * south;
    }

    private static int CountViewingDistance(IReadOnlyCollection<int> treesInDirectionOfView, int heightOfViewpoint)
    {
        if (treesInDirectionOfView.Count == 0) return 0;
        var viewingDistance = treesInDirectionOfView.TakeWhile(treeNorthOfMe => treeNorthOfMe < heightOfViewpoint).Count();
        //If all trees north are visible no tree stopped the loop
        if (viewingDistance == treesInDirectionOfView.Count) return viewingDistance;
        //Include the tree that stopped the selection loop 
        return viewingDistance + 1;
    }

    public int MaxScenicScore()
    {
        var maxScenicScore =0;
        for (var x = 0; x < GridWidth; x++)
        {
            for (var y = 0; y < GridHeight; y++)
            {
                var scenicScore = ScenicScore(x, y);
                if (scenicScore > maxScenicScore) maxScenicScore = scenicScore;
            }
        }

        return maxScenicScore;
    }
}