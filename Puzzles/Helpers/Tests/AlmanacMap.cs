using System.Collections;

namespace TestProject1.Helpers.Tests;

public class AlmanacMap : Tuple<long,long,long>
{
    public AlmanacMap(long item1, long item2, long item3) : base(item1, item2, item3)
    {
    }

    private long DestinationStart => Item1;
    
    private long RangeStart => Item2;
    private long RangeEnd => Item2 + Item3-1;

    public long Destination(long seedNumber)
    {
        if (!ForItem(seedNumber)) return seedNumber;
        return DestinationStart + seedNumber - RangeStart;
    }

    public bool ForItem(long item)
    {
        return (item >= RangeStart && item <= RangeEnd);
    }

    public IEnumerable<long> PossibleDestinations()
    {
        var result = new List<long>();
        for (var i = 0; i < Item3; i++)
        {
            result.Add(DestinationStart+i);
        }

        return result;
    }
}