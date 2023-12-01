using Microsoft.VisualBasic.FileIO;

namespace TestProject1.Helpers;

public static class PuzzleInput
{
    public const string BaseDirectory = "puzzleinput";

    static PuzzleInput()
    {
        if (!FileSystem.DirectoryExists(BaseDirectory))
        {
            FileSystem.CreateDirectory(BaseDirectory);
        }
    }
    
    public static IEnumerable<string> GetFile(string fileName)
    {
        var puzzleInputFile = Path.Combine(BaseDirectory, fileName);
        return File.ReadAllLines(puzzleInputFile);
    }
    
    public static IEnumerable<string> InputStringToArray(string stackString)
    {
        return stackString.Split(Environment.NewLine);
    }

    public static IEnumerable<IEnumerable<string>> GetPuzzleSegments(IEnumerable<string> puzzleInput,
        string segmentDelimiter)
    {
        var puzzleInputList = puzzleInput.ToList();
        puzzleInputList.Add(segmentDelimiter);
        
        IList<IEnumerable<string>> result = new List<IEnumerable<string>>();
        List<string> segment = new();
        foreach (var line in puzzleInputList)
        {
            if (line == segmentDelimiter)
            {
                if (segment.Any())
                {
                    result.Add(segment.ToArray());
                    segment.Clear();
                }
                continue;
            }
            segment.Add(line);
        }
        
        return result;
    }

    public static IEnumerable<Tuple<string,string>> GetPuzzlePairs(IEnumerable<string> input, string delimiter)
    {
        return input
            .Select(line => line.Split(delimiter))
            .Select(split => new Tuple<string, string>(split[0], split[1]))
            .ToList();
    }

    public static IEnumerable<Tuple<string, string>> GetPuzzleLinesSplit(IEnumerable<string> puzzleInputLines)
    {
        return puzzleInputLines.Select(line =>
        {
            var charArray = line.ToCharArray();
            var compartmentSize = charArray.Length/2;
            return new Tuple<string, string>(charArray.Take(compartmentSize).ToString()!,
                charArray.Skip(compartmentSize).Take(compartmentSize).ToString()!);
        });
    }
}