namespace TestProject1.Helpers;

public class ForestPuzzleGrid : PuzzleGrid
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
                if (IsTreeVisible(x, y)) visible.Add(GetItemAsInteger(x, y));
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
        var treeHeight = GetItemAsInteger(x, y);

        var treesNorthOfMe = GridCompass.AllItemsNorth(x, y);

        return (treesNorthOfMe.All(north => north < treeHeight));
    }

    private bool VisibleSouth(int x, int y)
    {
        var treeHeight = GetItemAsInteger(x, y);
        var treesSouthOfMe = GridCompass.AllItemsSouth(x, y);
        return (treesSouthOfMe.All(south => south < treeHeight));
    }

    private bool VisibleEast(int x, int y)
    {
        var treeHeight = GetItemAsInteger(x, y);
        var treesEastOfMe = GridCompass.AllItemsEast(x, y);
        return (treesEastOfMe.All(east => east < treeHeight));
    }

    private bool VisibleWest(int x, int y)
    {
        var treeHeight = GetItemAsInteger(x, y);
        var treesWestOfMe = GridCompass.AllItemsWest(x, y);
        return (treesWestOfMe.All(west => west < treeHeight));
    }

    public int ViewingDistanceNorth(int x, int y)
    {
        var treeHeight = GetItemAsInteger(x, y);
        var treesInDirectionOfView = GridCompass.AllItemsNorth(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }

    public int ViewingDistanceSouth(int x, int y)
    {
        var treeHeight = GetItemAsInteger(x, y);
        var treesInDirectionOfView = GridCompass.AllItemsSouth(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }

    public int ViewingDistanceEast(int x, int y)
    {
        var treeHeight = GetItemAsInteger(x, y);
        var treesInDirectionOfView = GridCompass.AllItemsEast(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }

    public int ViewingDistanceWest(int x, int y)
    {
        var treeHeight = GetItemAsInteger(x, y);
        var treesInDirectionOfView = GridCompass.AllItemsWest(x, y).ToArray();
        return CountViewingDistance(treesInDirectionOfView, treeHeight);
    }

    public int ScenicScore(int x, int y)
    {
        
        var treeHeight = GetItemAsInteger(x, y);
        
        var north = CountViewingDistance(GridCompass.AllItemsNorth(x, y).ToArray(), treeHeight);
        var west = CountViewingDistance(GridCompass.AllItemsWest(x, y).ToArray(), treeHeight);
        var east = CountViewingDistance(GridCompass.AllItemsEast(x, y).ToArray(), treeHeight);
        var south = CountViewingDistance(GridCompass.AllItemsSouth(x, y).ToArray(), treeHeight);

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