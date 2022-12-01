namespace TestProject1.Helpers;

public class CalorieCounter
{
    public static IEnumerable<int> OrderedCalories(IEnumerable<string> input)
    {
        var snacks = EnsureListHasEmptyLastElement(input);
        var elves = new List<int>();
        var elfSum = 0;
        foreach (var snack in snacks)
        {
            if (snack == string.Empty)
            {
                elves.Add(elfSum);
                elfSum = 0;
                continue;
            }

            elfSum += int.Parse(snack);
        }

        return elves.OrderByDescending(i => i);
    }

    public static int MostCalories(IEnumerable<string> input) => OrderedCalories(input).First();

    private static List<string> EnsureListHasEmptyLastElement(IEnumerable<string> input)
    {
        var enumerable = input.ToList();
        enumerable.Add("");
        return enumerable;
    }
}