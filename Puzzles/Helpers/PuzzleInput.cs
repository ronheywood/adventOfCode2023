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
    
    public static IEnumerable<string> InputStringToArray(string stackString)
    {
        Stream ms = new MemoryStream(Encoding.UTF8.GetBytes(stackString));
        var stackReader =
            new StreamReader(ms, Encoding.UTF8, true);
        var lines = new List<string>();
        while (true)
        {
            var line = stackReader.ReadLine();
            if (line == null)
            {
                return lines;
            }

            lines.Add(line);
        }
    }
}