namespace TestProject1.Helpers;

public class IntPuzzleGrid
{
    protected readonly GridCompass GridCompass;

    public IntPuzzleGrid(IEnumerable<string> grid)
    {
        Items = new List<string>();
        var rows = grid.Select(row => row.ToCharArray()).ToArray();
        GridHeight = rows.Length;
        foreach (var row in rows)
        {
            var items = row.Select(char.ToString);
            GridWidth = row.Length;
            Items.AddRange(items);
        }

        GridCompass = new GridCompass(this);
    }

    public List<string> Items { get; set; }

    public int GridWidth { get; private set; }

    public int GridHeight { get; private set; }

    public int GetItem(int x, int y)
    {
        return GridCompass.GetItemAsInteger(x, y);
    }
}