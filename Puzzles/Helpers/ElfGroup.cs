namespace TestProject1.Helpers;

public class ElfGroup
{
    public static IEnumerable<IEnumerable<string>> FromInventory(IEnumerable<string> ruckSackCollection)
    {
        return Split(ruckSackCollection.ToList());
    }
    private static List<List<T>> Split<T>(IList<T> source)
    {
        return  source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / 3)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }

    public static char BadgeCode(IEnumerable<string> group)
    {
        var enumerable = group as string[] ?? group.ToArray();
        if (enumerable.Count() != 3) throw new ArgumentException("A group must include 3 rucksacks");
        var group1 = enumerable[0];
        var group2 = enumerable[1];
        var group3 = enumerable[2];
        var commmon = group1.Intersect(group2);
        return group3.First(x => commmon.Contains(x));
    }
}