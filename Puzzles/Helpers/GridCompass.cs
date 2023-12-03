using NUnit.Framework.Constraints;

namespace TestProject1.Helpers;

public class GridCompass
{
    private readonly IntPuzzleGrid _grid;

    public GridCompass(IntPuzzleGrid grid)
    {
        _grid = grid;
    }

    public IEnumerable<int> AllItemsNorth(int x, int y)
    {
        if (y == 0) return Enumerable.Empty<int>();

        var result = new List<int>();

        for (var row = y; row > 0; row--)
        {
            result.Add(_grid.GetItem(x, row - 1));
        }

        return result;
    }

    public IEnumerable<int> AllItemsSouth(int x, int y)
    {
        if (y >= _grid.GridHeight - 1)
            return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var row = y; row < _grid.GridHeight - 1; row++)
        {
            result.Add(_grid.GetItem(x, row + 1));
        }

        return result;
    }

    public IEnumerable<int> AllItemsWest(int x, int y)
    {
        if (x == 0) return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var column = x; column > 0; column--)
        {
            result.Add(_grid.GetItem(column - 1, y));
        }
        return result;
    }

    public IEnumerable<int> AllItemsEast(int x, int y)
    {
        if (x == _grid.GridWidth - 1)
            return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var column = x; column < _grid.GridWidth - 1; column++)
            result.Add(_grid.GetItem(column + 1, y));
        return result;
    }

    public int GetItemAsInteger(int x, int y)
    {
        var item = GetItem(x, y);
        return int.TryParse(item, out var integer) ? integer : 0;
    }

    public string GetItem(int x, int y)
    {
        var column = y * _grid.GridWidth;
        var rowStart = _grid.Items.Skip(column);
        var item = rowStart.Skip(x).First();
        return item;
    }

    public string? NorthWestNeighbor(int x, int y)
    {
        var allItems = AllItemsWest(x, y-1).ToArray();
        return allItems.Any() ? allItems[0].ToString() : null;
    }

    public string? SouthWestNeighbor(int x, int y)
    {
        var allItems = AllItemsWest(x, y+1).ToArray();
        return allItems.Any() ? allItems[0].ToString() : null;
    }

    public string? SouthEastNeighbor(int x, int y)
    {
        var allItems = AllItemsEast(x, y+1).ToArray();
        return allItems.Any() ? allItems[0].ToString() : null;
    }

    public string? EastNeighbor(int x, int y)
    {
        var allItems = AllItemsEast(x, y).ToArray();
        return allItems.Any() ? allItems[0].ToString() : null;
    }

    public string? WestNeighbor(int x, int y)
    {
        var allItems = AllItemsWest(x, y).ToArray();
        return allItems.Any() ? allItems[0].ToString() : null;
    }

    public string? NorthEastNeighbor(int x, int y)
    {
        
        var allItems = AllItemsEast(x, y-1).ToArray();
        return allItems.Any() ? allItems[0].ToString() : null;
    }

    public string? NorthNeighbor(int x, int y)
    {
        var allItemsNorth = AllItemsNorth(x, y).ToArray();
        return allItemsNorth.Any() ? allItemsNorth[0].ToString() : null;
    }

    public string? SouthNeighbor(int x, int y)
    {
        var allItemsSouth = AllItemsSouth(x, y).ToArray();
        return allItemsSouth.Any() ? allItemsSouth[0].ToString() : null;
    }
}