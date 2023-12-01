namespace TestProject1.Helpers;

public class TrebuchetCalibration
{
    private static Dictionary<string, string> _lookup = new Dictionary<string, string>()
    {
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" }
    };

    public static string Calibrate(string input)
    {
        
        return FirstWordOrNumber(input) + LastWordOrNumber(input);
        
    }

    public static int SumCalibration(IEnumerable<string> lines)
    {
        var numbers = lines.Select(Calibrate).ToList();
        if (numbers.Any(number => !int.TryParse(number,out _)))
        {
            throw new Exception($"A line has no numbers");
        }
        return numbers.Select(int.Parse).Sum();
    }

    public static string FirstWordOrNumber(string line)
    {
        if (line == string.Empty) return string.Empty;
        if(_lookup.ContainsValue(line[0].ToString()))
        {
            return line[0].ToString();
        }

        var firstWordAsNumber = _lookup.Where(kvp => line.StartsWith(kvp.Key)).Select(kvp => kvp.Value).FirstOrDefault();
        return firstWordAsNumber ?? FirstWordOrNumber(line.Substring(1,line.Length-1));
    }

    public static string LastWordOrNumber(string line)
    {
        if (string.IsNullOrEmpty(line)) return string.Empty;
        if(_lookup.ContainsValue(line[^1].ToString()))
        {
            return line[^1].ToString();
        }

        var lastWordAsNumber = _lookup.Where(kvp => line.EndsWith(kvp.Key)).Select(kvp => kvp.Value).FirstOrDefault();
        
        return lastWordAsNumber ?? 
               LastWordOrNumber(line[..^1]); //range indexer - equivalent to Substring(0,line.Length-1)
    }
}