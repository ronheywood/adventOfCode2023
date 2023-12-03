using NUnit.Framework.Constraints;

namespace TestProject1.Helpers;

public class GridCompass
{
    private readonly PuzzleGrid _grid;

    public GridCompass(PuzzleGrid grid)
    {
        _grid = grid;
    }

    public IEnumerable<int> AllItemsNorth(int x, int y)
    {
        if (y == 0) return Enumerable.Empty<int>();

        var result = new List<int>();

        for (var row = y; row > 0; row--)
        {
            result.Add(_grid.GetItemAsInteger(x, row - 1));
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
            result.Add(_grid.GetItemAsInteger(x, row + 1));
        }

        return result;
    }

    public IEnumerable<int> AllItemsWest(int x, int y)
    {
        if (x == 0) return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var column = x; column > 0; column--)
        {
            result.Add(_grid.GetItemAsInteger(column - 1, y));
        }
        return result;
    }

    public IEnumerable<int> AllItemsEast(int x, int y)
    {
        if (x == _grid.GridWidth - 1)
            return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var column = x; column < _grid.GridWidth - 1; column++)
            result.Add(_grid.GetItemAsInteger(column + 1, y));
        return result;
    }

    public int GetItemAsInteger(int x, int y)
    {
        var item = GetItem(x, y);
        return int.TryParse(item, out var integer) ? integer : 0;
    }

    public string? GetItem(int x, int y)
    {
        var column = y * _grid.GridWidth;
        var rowStart = _grid.Items.Skip(column);
        var item = rowStart.Skip(x).FirstOrDefault();
        return item;
    }

    public string? NorthWestNeighbor(int x, int y)
    {
        if (x == 0) return null;
        if (y == 0) return null;
        return GetItem(x - 1, y - 1);
    }

    public string? SouthWestNeighbor(int x, int y)
    {
        if (x == 0) return null;
        if (y == _grid.GridHeight) return null;
        return GetItem(x-1, y+1);
    }

    public string? SouthEastNeighbor(int x, int y)
    {
        if (x == _grid.GridWidth-1) return null;
        if (y == _grid.GridHeight) return null;
        return GetItem(x+1, y+1);
    }

    public string? EastNeighbor(int x, int y)
    {
        if (x == _grid.GridWidth - 1) return null;
        return GetItem(x+1, y);
    }

    public string? WestNeighbor(int x, int y)
    {
        if (x == 0) return null;
        return GetItem(x-1, y);
    }

    public string? NorthEastNeighbor(int x, int y)
    {
        if (y == 0) return null;
        return GetItem(x+1, y-1);
    }

    public string? NorthNeighbor(int x, int y)
    {
        if (y == 0) return null;
        return GetItem(x, y-1);
    }

    public string? SouthNeighbor(int x, int y)
    {
        return GetItem(x, y+1);
    }
}