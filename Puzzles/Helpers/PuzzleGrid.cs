namespace TestProject1.Helpers;

public class PuzzleGrid
{
    protected readonly GridCompass GridCompass;

    public PuzzleGrid(IEnumerable<string> grid)
    {
        var gridRows = grid as string[] ?? grid.ToArray();
        GridRows = gridRows;
        Items = new List<string>();
        var rows = gridRows.Select(row => row.ToCharArray()).ToArray();
        GridHeight = rows.Length;
        foreach (var row in rows)
        {
            var items = row.Select(char.ToString);
            GridWidth = row.Length;
            Items.AddRange(items);
        }

        GridCompass = new GridCompass(this);
    }

    protected IEnumerable<string> GridRows { get; set; }

    public List<string> Items { get; }

    public int GridWidth { get; private set; }

    public int GridHeight { get; private set; }

    public int GetItemAsInteger(int x, int y)
    {
        return int.TryParse(GridCompass.GetItem(x, y),out var asInt) ? asInt : 0;
    }
}