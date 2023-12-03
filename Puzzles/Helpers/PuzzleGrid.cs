using System.Collections;
using System.Text.RegularExpressions;

namespace TestProject1.Helpers;

public class PuzzleGrid
{
    protected readonly GridCompass GridCompass;

    public PuzzleGrid(IEnumerable<string> grid)
    {
        GridRows = grid;
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

    public IEnumerable<string> GridRows { get; set; }

    public List<string> Items { get; }

    public int GridWidth { get; private set; }

    public int GridHeight { get; private set; }

    public string? GetItem(int x, int y)
    {
        return GridCompass.GetItem(x, y);
    }

    public int GetItemAsInteger(int x, int y)
    {
        return int.TryParse(GridCompass.GetItem(x, y),out var asInt) ? asInt : 0;
    }
}

class EngineSchematicGrid : PuzzleGrid
{
    public EngineSchematicGrid(IEnumerable<string> grid) : base(grid)
    {
    }

    public IEnumerable<Tuple<int,int>> FindParts()
    {
        var result = new List<Tuple<int, int>>();
        for (var index = 0; index < GridRows.Count(); index++)
        { 
            var item = GridRows.Skip(index).First();
            var charArray = item.ToCharArray();
            
            for (var x = 0; x < charArray.Length; x++)
            {
                var character = charArray[x];
                if (!CharIsSchematicCharacter(character)) continue;
                result.Add(new Tuple<int, int>(x, index));
            }
        }

        return result;
    }

    private static bool CharIsSchematicCharacter(char c)
    {
        return c.ToString() != "." && !char.IsNumber(c);
    }

    public List<Tuple<int,int, int>> FindNumbers()
    {
        var result = new List<Tuple<int,int,int>>();
        for (var y = 0; y < GridRows.Count(); y++)
        {
            var line = GridRows.Skip(y).First().ToCharArray();
            if (!line.Any(char.IsNumber)) continue;
            var offset = 0;
            while( GetNumberTuple(line, y, offset,out var tuple) && offset < line.Length){
                result.Add(tuple);
                offset = tuple.Item1 + tuple.Item3.ToString().Length+1;
            }
        }

        return result;
    }

    private static bool GetNumberTuple(char[] line, int y, int offset, out Tuple<int,int,int> tuple)
    {
        var numberStartIndex = 0;
        tuple = null!;
        try
        {
            numberStartIndex = line
                .Skip(offset)
                .Select((value, index) => new { value, index })
                .SkipWhile(pair => !char.IsNumber(pair.value))
                .Select(pair => pair.index + offset).First();
        }
        catch (Exception)
        {
            return false;
        }

        if (numberStartIndex > line.Length) return false;
        
        var numbers = line.Skip(numberStartIndex).TakeWhile(char.IsNumber);
        
        var numberString = string.Join("", numbers);
        var number = int.TryParse(numberString, out var numberResult) ? numberResult : 0;
        
        tuple = new Tuple<int, int, int>(numberStartIndex, y, number);
        return true;
    }

    public IEnumerable FilterPartNumbers(List<Tuple<int,int,int>> numbers)
    {
        var result = new List<int>();
        foreach (var numberTuple in numbers)
        {
            var (x, y, number) = numberTuple;
            
            for (var offset = 0; offset < number.ToString().Length; offset++)
            {
                var north = GridCompass.NorthNeighbor(x+offset, y) ?? ".";
                var northEast = GridCompass.NorthEastNeighbor(x+offset, y) ?? ".";
                var northWest = GridCompass.NorthWestNeighbor(x+offset, y) ?? ".";
                var south = GridCompass.SouthNeighbor(x+offset, y) ?? ".";
                var southEast = GridCompass.SouthEastNeighbor(x+offset, y) ?? ".";
                var southWest = GridCompass.SouthWestNeighbor(x+offset, y) ?? ".";
                var east = GridCompass.EastNeighbor(x + number.ToString().Length - 1, y) ?? ".";
                var west = GridCompass.WestNeighbor(x, y) ?? ".";
                if (north != "." && int.TryParse(north, out var _) == false)
                {
                    result.Add(number);
                    break;
                }

                if (northEast != "." && int.TryParse(north, out var _) == false)
                {
                    result.Add(number);
                    break;
                }

                if (northWest != "." && int.TryParse(north, out var _) == false)
                {
                    result.Add(number);
                    break;
                }

                if (south != "." && int.TryParse(north, out var _) == false)
                {
                    result.Add(number);
                    break;
                }

                if (southWest != "." && int.TryParse(north, out var _) == false)
                {
                    result.Add(number);
                    break;
                }

                if (southEast != "." && int.TryParse(north, out var _) == false)
                {
                    result.Add(number);
                    break;
                }

                if (east != "." && int.TryParse(north, out var _) == false)
                {
                    result.Add(number);
                    break;
                }

                if (west != "." && int.TryParse(north, out var _) == false)
                {
                    result.Add(number);
                    break;
                }
            }
        }

        return result;
    }
}