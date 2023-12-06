namespace TestProject1.Helpers.Tests;

public class Almanac
{
    public Almanac(IEnumerable<string> puzzleInput, bool useRangeOfSeeds = false)
    {
        var puzzleLines = puzzleInput as string[] ?? puzzleInput.ToArray();

        if (useRangeOfSeeds)
        {
            Seeds = new List<uint>();
            SeedRanges = InitSeedRanges(puzzleLines);
        }
        else {
            Seeds = InitSeedsCollection(puzzleLines);
            SeedRanges = Enumerable.Empty<Tuple<long, long>>();
        }
        SeedToSoilMap = InitMap(puzzleLines, "seed-to-soil map:");
        SoilToFertilizerMap = InitMap(puzzleLines, "soil-to-fertilizer map:");
        FertilizerToWaterMap = InitMap(puzzleLines, "fertilizer-to-water map:");
        WaterToLightMap = InitMap(puzzleLines, "water-to-light map:");
        LightToTemperatureMap = InitMap(puzzleLines, "light-to-temperature map:");
        TemperatureToHumidityMap = InitMap(puzzleLines, "temperature-to-humidity map:");
        HumidityToLocationMap = InitMap(puzzleLines, "humidity-to-location map:");
    }

    private IEnumerable<Tuple<long,long>> InitSeedRanges(string[] puzzleLines)
    {
        var result = new List<Tuple<long,long>>();
        var seedsString = SeedsString(puzzleLines).Split(" ").ToArray();
        for (var i = 0; i < seedsString.Length;i+=2)
        {
            var rangeSize = long.Parse(seedsString[i+1]);
            var rangeStart = long.Parse(seedsString[i]);
            var tuple = new Tuple<long, long>(rangeStart, rangeSize+rangeStart-1);
            result.Add(tuple);
        }

        return result;
    }

    public long ChainOfMapping(long seed)
    {
        var soil = Destination(seed, SeedToSoilMap.ToArray());
        var fertilizer = Destination(soil, SoilToFertilizerMap.ToArray());
        var water = Destination(fertilizer,FertilizerToWaterMap.ToArray());
        var light = Destination(water, WaterToLightMap.ToArray());
        var temp = Destination(light, LightToTemperatureMap.ToArray());
        var humid = Destination(temp, TemperatureToHumidityMap.ToArray());
        return Destination(humid, HumidityToLocationMap.ToArray());
    }

    private static IEnumerable<AlmanacMap> InitMap(IEnumerable<string> puzzleLines, string mapHeader)
    {
        var stringMap = puzzleLines.Skip(1).SkipWhile(s => !s.Contains(mapHeader)).Skip(1)
            .TakeWhile(s => !string.IsNullOrWhiteSpace(s));
        return MakeAlmanacMap(stringMap);
    }

    private static IEnumerable<uint> InitSeedsCollection(IEnumerable<string> puzzleLines)
    {
        var seedsString = SeedsString(puzzleLines);
        return seedsString.Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).Select(uint.Parse);
    }

    private static string SeedsString(IEnumerable<string> puzzleLines)
    {
        var seed = puzzleLines.First();
        var seedsString = seed.Split(':').Last().Trim();
        return seedsString;
    }

    private static IEnumerable<AlmanacMap> MakeAlmanacMap(IEnumerable<string> mapAsStringList)
    {
        var intMap = mapAsStringList.Select(s => s.Split(" ").Where(s1 => !string.IsNullOrWhiteSpace(s1)));
        
        return intMap.Select(mapItem => mapItem
                .Select(s => s.Trim()).Select(long.Parse).ToArray())
            .Select(mapItemInt =>
                new AlmanacMap(mapItemInt[0], mapItemInt[1], mapItemInt[2])
            );
    }

    public IEnumerable<uint> Seeds { get; }

    public IEnumerable<AlmanacMap> SeedToSoilMap { get; }

    public IEnumerable<AlmanacMap> SoilToFertilizerMap { get; }

    public IEnumerable<AlmanacMap> FertilizerToWaterMap { get; }

    public IEnumerable<AlmanacMap> WaterToLightMap { get; }

    public IEnumerable<AlmanacMap> LightToTemperatureMap { get; }

    public IEnumerable<AlmanacMap> HumidityToLocationMap { get; }

    public IEnumerable<AlmanacMap> TemperatureToHumidityMap { get; }

    public IEnumerable<Tuple<long,long>> SeedRanges { get; }

    public static long Destination(long seedNumber, IEnumerable<AlmanacMap> map)
    {
        var almanacMap = map.FirstOrDefault(m => m.ForItem(seedNumber));
        return almanacMap?.Destination(seedNumber) ?? seedNumber;
    }
}