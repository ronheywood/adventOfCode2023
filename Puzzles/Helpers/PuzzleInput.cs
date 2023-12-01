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
}