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
}