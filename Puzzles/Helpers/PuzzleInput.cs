using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace TestProject1.Helpers;

public class PuzzleInput
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
        var puzzleInputFile = Path.Combine("puzzleinput", fileName);
        List<string> lines = new ();
        using var fp = FileSystem.OpenTextFileReader(puzzleInputFile,Encoding.Default);
        while (true)
        {
            var line = fp.ReadLine();
            if (line == null)
            {
                return lines;
            }

            lines.Add(line);
        }
    }
}