using NUnit.Framework.Constraints;

namespace TestProject1.Helpers;

public class IntPuzzleGrid
{
    protected readonly GridCompass _gridCompass;

    public IntPuzzleGrid(IEnumerable<string> grid)
    {
        Items = new List<int>();
        var rows = grid.Select(row => row.ToCharArray()).ToArray();
        GridHeight = rows.Length;
        foreach (var row in rows)
        {
            var items = row.Select(i => int.Parse(char.ToString(i)));
            GridWidth = row.Length;
            Items.AddRange(items);
        }

        _gridCompass = new GridCompass(this);
    }

    public List<int> Items { get; set; }

    public int GridWidth { get; private set; }

    public int GridHeight { get; private set; }

    public int GetItem(int x, int y)
    {
        return _gridCompass.GetItem(x, y);
    }
}