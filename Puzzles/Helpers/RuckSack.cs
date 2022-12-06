using System.Text.RegularExpressions;

namespace TestProject1.Helpers;

public class RuckSack
{
    public static IEnumerable<IEnumerable<char>> Inventory(string contents)
    {
        var contentsArray = contents.ToCharArray();
        if((contentsArray.Length % 2) > 0)
            throw new Exception("Uneven number of items");
        
        var regex = new Regex(@"^[a-zA-Z]*$");
        if (!regex.IsMatch(contents))
            throw new Exception("Unexpected item code");

        var compartmentSize = contentsArray.Length/2;
        return new[] { contentsArray.Take(compartmentSize), contentsArray.Skip(compartmentSize).Take(compartmentSize) };
    }

    public static char FindDuplicate(IEnumerable<char> compartmentOneContents, IEnumerable<char> compartmentTwoContents)
    {
        return compartmentOneContents.FirstOrDefault(compartmentTwoContents.Contains);
    }

    public static int Priority(char c)
    {
        var iValue = (c-0);
        if (iValue >= 97) return iValue - 96;
        if (iValue >= 65) return iValue - 64 + 26;
        
        throw new Exception("Unexpected item code");
    }

    public static int GetPriorty(string contents)
    {
        var compartmentContents = Inventory(contents).ToArray();
        var duplicate = FindDuplicate(compartmentContents[0],compartmentContents[1]);
        return Priority(duplicate);
    }
}