namespace TestProject1.Helpers;

public class PipePuzzleGrid : PuzzleGrid
{
    public PipePuzzleGrid(IEnumerable<string> map) : base(map)
    {
    }

    public Tuple<int, int> StartLocation()
    {
        for (var x = 0; x < GridWidth; x++)
        {
            for (var y = 0; y < GridHeight; y++)
            {
                if (GridCompass.GetItem(x, y) == "S") return new Tuple<int, int>(x, y);
            }
        }

        throw new Exception("StartLocation not found");
    }

    public string DistancePlot()
    {
        var response = string.Empty;
        var column = 0;
        var startFound = false;
        foreach (var item in Items)
        {
            if (item == "S") startFound = true;
            var itemString = ".";
            if (startFound)
            {
                itemString = (item == "S") ? "0" : item;
            }

            response += itemString;
            if (column == GridWidth - 1)
            {
                response += "\r\n";
                column = 0;
            }
            else
            {
                column++;
            }
        }

        return response;
    }
}