namespace TestProject1.Helpers;

class ForestGridCompass : GridCompass
{
    public ForestGridCompass(IntPuzzleGrid grid) : base(grid)
    {
    }
}

class ForestPuzzleGrid : IntPuzzleGrid
{
    public ForestPuzzleGrid(IEnumerable<string> grid) : base(grid)
    {
    }

    public IEnumerable<int> VisibleTrees()
    {
        var visible = new List<int>();
        for (var x = 0; x < GridWidth; x++)
        {
            for (var y = 0; y < GridHeight; y++)
            {
                if (IsTreeVisible(x, y)) visible.Add(GetItem(x, y));
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

    private bool VisibleNorth(int x, int y)
    {
        var treeHeight = GetItem(x, y);

        var treesNorthOfMe = _gridCompass.North(x, y);

        return (treesNorthOfMe.All(north => north < treeHeight));
    }

    private bool VisibleSouth(int x, int y)
    {
        var treeHeight = GetItem(x, y);
        var treesSouthOfMe = _gridCompass.South(x, y);
        return (treesSouthOfMe.All(south => south < treeHeight));
    }

    private bool VisibleEast(int x, int y)
    {
        var treeHeight = GetItem(x, y);
        var treesEastOfMe = _gridCompass.East(x, y);
        return (treesEastOfMe.All(east => east < treeHeight));
    }

    private bool VisibleWest(int x, int y)
    {
        var treeHeight = GetItem(x, y);
        var treesWestOfMe = _gridCompass.West(x, y);
        return (treesWestOfMe.All(west => west < treeHeight));
    }

    public int ViewingDistanceNorth(int x, int y)
    {
        var treeHeight = GetItem(x, y);
        var treesInDirectionOfView = _gridCompass.North(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }

    public int ViewingDistanceSouth(int x, int y)
    {
        var treeHeight = GetItem(x, y);
        var treesInDirectionOfView = _gridCompass.South(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }

    public int ViewingDistanceEast(int x, int y)
    {
        var treeHeight = GetItem(x, y);
        var treesInDirectionOfView = _gridCompass.East(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }

    public int ViewingDistanceWest(int x, int y)
    {
        var treeHeight = GetItem(x, y);
        var treesInDirectionOfView = _gridCompass.West(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }

    public int ScenicScore(int x, int y)
    {
        
        var treeHeight = GetItem(x, y);
        
        var north = CountViewingDistance(_gridCompass.North(x, y).ToArray(), treeHeight);
        var west = CountViewingDistance(_gridCompass.West(x, y).ToArray(), treeHeight);
        var east = CountViewingDistance(_gridCompass.East(x, y).ToArray(), treeHeight);
        var south = CountViewingDistance(_gridCompass.South(x, y).ToArray(), treeHeight);

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