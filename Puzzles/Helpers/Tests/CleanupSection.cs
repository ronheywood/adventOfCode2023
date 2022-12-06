using NUnit.Framework.Constraints;

namespace TestProject1.Helpers.Tests;

public class CleanupSection
{
    public static IEnumerable<int> RangeToSector(string range)
    {
        var rangeStartEnd = range.Split("-");
        var end = int.Parse(rangeStartEnd[1]);
        var start = int.Parse(rangeStartEnd[0]);
        var result = new List<int>();
        for (var i = start; i <= end; i++)
        {
            result.Add(i);
        }

        return result;
    }
    
    public static Tuple<IEnumerable<int>,IEnumerable<int>> Sectors(string inputLine1, string inputLine2)
    {
        var section1 = inputLine1.ToCharArray().Where(x => int.TryParse(x.ToString(), out var y)).Select(x => int.Parse(x.ToString()));
        var section2 = inputLine2.ToCharArray().Where(x => int.TryParse(x.ToString(), out var y)).Select(x => int.Parse(x.ToString()));
        return new Tuple<IEnumerable<int>, IEnumerable<int>>(section1, section2);
    }

    public static bool Validate(int[] section1, int[] section2)
    {   
        var section1IsCoveredBySection2 = section1.All(section2.Contains);
        var section2IsCoveredBySection1 = section2.All(section1.Contains);
        
        return !section1IsCoveredBySection2 && !section2IsCoveredBySection1;
    }

    public static IEnumerable<Tuple<IEnumerable<int>,IEnumerable<int>>> Sections(IEnumerable<string> input)
    {
        var result = new List<Tuple<IEnumerable<int>,IEnumerable<int>>>();
        var inputArray = input as string[] ?? input.ToArray();
        for (var i = 0; i < inputArray.Length; i++)
        {
            var section1 = inputArray[i];
            if (string.IsNullOrEmpty(section1))
            {
                i++;
                section1 = inputArray[i];
            }

            i++;
            var section2 = inputArray[i];
            result.Add( Sectors(section1,section2) );
        }   
        return result;
    }

    public static bool ValidateIsolated(int[] section1, int[] section2)
    {
        return !section1.Any(section2.Contains);
    }
}