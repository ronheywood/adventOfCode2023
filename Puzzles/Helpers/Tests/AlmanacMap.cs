namespace TestProject1.Helpers.Tests;

public class AlmanacMap : Tuple<long,long,long>
{
    public AlmanacMap(long item1, long item2, long item3) : base(item1, item2, item3)
    {
    }

    private long RangeStart => Item2;
    private long RangeEnd => Item2 + Item3-1;

    public long Destination(long seedNumber)
    {
        if (!ForItem(seedNumber)) return seedNumber;
        return Item1 + seedNumber - RangeStart;
    }

    public bool ForItem(long item)
    {
        return (item >= RangeStart && item <= RangeEnd);
    }
}