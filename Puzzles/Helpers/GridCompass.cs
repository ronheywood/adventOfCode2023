namespace TestProject1.Helpers;

public class GridCompass
{
    private readonly IntPuzzleGrid _grid;

    public GridCompass(IntPuzzleGrid grid)
    {
        _grid = grid;
    }

    public IEnumerable<int> North(int x, int y)
    {
        if (y == 0) return Enumerable.Empty<int>();

        var result = new List<int>();

        for (var row = y; row > 0; row--)
        {
            result.Add(_grid.GetItem(x, row - 1));
        }

        return result;
    }

    public IEnumerable<int> South(int x, int y)
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

    public IEnumerable<int> West(int x, int y)
    {
        if (x == 0) return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var column = x; column > 0; column--)
        {
            result.Add(_grid.GetItem(column - 1, y));
        }
        return result;
    }

    public IEnumerable<int> East(int x, int y)
    {
        if (x == _grid.GridWidth - 1)
            return Enumerable.Empty<int>();
        var result = new List<int>();
        for (var column = x; column < _grid.GridWidth - 1; column++)
            result.Add(_grid.GetItem(column + 1, y));
        return result;
    }

    public int GetItem(int x, int y)
    {
        var column = y * _grid.GridWidth;
        var rowStart =  _grid.Items.Skip(column);
        return rowStart.Skip(x).First();
    }
}