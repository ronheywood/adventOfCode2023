namespace TestProject1.Helpers;

public class CalorieCounter
{
    public static IEnumerable<int> OrderedCalories(IEnumerable<string> input)
    {
        //Each Elf separates their own inventory from the previous Elf's inventory
        //(if any) by a blank line
        var elves = PuzzleInput.GetPuzzleSegments(input,"").ToArray();
        var calories = new List<int>();
        foreach (var elf in elves)
        {
            calories.Add(elf.Select(int.Parse).Sum());
        }

        return calories.OrderByDescending(i => i);
    }

    public static int MostCalories(IEnumerable<string> input) => OrderedCalories(input).First();
}