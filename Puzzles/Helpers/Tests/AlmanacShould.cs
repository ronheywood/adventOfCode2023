namespace TestProject1.Helpers.Tests;

public class AlmanacShould
{
    [Test]
    public void Extract_list_of_seeds()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        var expected = new[] { 79, 14, 55, 13 };
        CollectionAssert.AreEqual(expected, almanac.Seeds);

        almanac = new Almanac(PuzzleInput.InputStringToArray("seeds:  1  2 55 13"));
        expected = new[] { 1, 2, 55, 13 };
        CollectionAssert.AreEqual(expected, almanac.Seeds,
            "Puzzle input often uses fixed width columns and single digits can be odd");
    }
    
    [Test]
    public async Task Extract_range_of_seeds()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput),true);
        await Verify(almanac.Seeds);
    }
    
    [Test]
    public async Task Extract_many_ranges_of_seeds()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(@"seeds: 1 2 3 2 4 2"),true);
        await Verify(almanac.Seeds);
    }

    [Test]
    public void Extract_seed_to_soil_map()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));

        AlmanacMap soilMap1 = new(50, 98, 2);
        AlmanacMap soilMap2 = new(52, 50, 48);
        Assert.Multiple(() =>
        {
            Assert.That(almanac.SeedToSoilMap.Count, Is.EqualTo(2));
            Assert.That(almanac.SeedToSoilMap.First, Is.EqualTo(soilMap1));
            Assert.That(almanac.SeedToSoilMap.Last, Is.EqualTo(soilMap2));
        });
    }

    [Test]
    public void Extract_soil_to_fertilizer_map()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));

        AlmanacMap map1 = new(0, 15, 37);
        AlmanacMap map2 = new(37, 52, 2);
        AlmanacMap map3 = new(39, 0, 15);
        var almanacSeedToFertilizer = almanac.SoilToFertilizerMap.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(almanacSeedToFertilizer.Count, Is.EqualTo(3));
            Assert.That(almanacSeedToFertilizer[0], Is.EqualTo(map1));
            Assert.That(almanacSeedToFertilizer[1], Is.EqualTo(map2));
            Assert.That(almanacSeedToFertilizer[2], Is.EqualTo(map3));
        });
    }

    [Test]
    public void Extract_fertilizer_to_water_map()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));

        AlmanacMap map1 = new(49, 53, 8);
        AlmanacMap map2 = new(0, 11, 42);
        AlmanacMap map3 = new(42, 0, 7);
        AlmanacMap map4 = new(57, 7, 4);

        var almanacSeedToFertilizer = almanac.FertilizerToWaterMap.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(almanacSeedToFertilizer.Count, Is.EqualTo(4));
            Assert.That(almanacSeedToFertilizer[0], Is.EqualTo(map1));
            Assert.That(almanacSeedToFertilizer[1], Is.EqualTo(map2));
            Assert.That(almanacSeedToFertilizer[2], Is.EqualTo(map3));
            Assert.That(almanacSeedToFertilizer[3], Is.EqualTo(map4));
        });
    }

    [Test]
    public void Extract_water_to_light_map()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));

        AlmanacMap map1 = new(88, 18, 7);
        AlmanacMap map2 = new(18, 25, 70);
        var map = almanac.WaterToLightMap.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(map.Count, Is.EqualTo(2));
            Assert.That(map[0], Is.EqualTo(map1));
            Assert.That(map[1], Is.EqualTo(map2));
        });
    }

    [Test]
    public void Extract_light_to_temperature_map()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));

        AlmanacMap map1 = new(45, 77, 23);
        AlmanacMap map2 = new(81, 45, 19);
        AlmanacMap map3 = new(68, 64, 13);

        var map = almanac.LightToTemperatureMap.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(map.Count, Is.EqualTo(3));
            Assert.That(map[0], Is.EqualTo(map1));
            Assert.That(map[1], Is.EqualTo(map2));
            Assert.That(map[2], Is.EqualTo(map3));
        });
    }

    [Test]
    public void Extract_temperature_to_humidity_map()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));

        AlmanacMap map1 = new(0, 69, 1);
        AlmanacMap map2 = new(1, 0, 69);

        var map = almanac.TemperatureToHumidityMap.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(map.Count, Is.EqualTo(2));
            Assert.That(map[0], Is.EqualTo(map1));
            Assert.That(map[1], Is.EqualTo(map2));
        });
    }

    [Test]
    public void Extract_humidity_to_location_map()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        AlmanacMap map1 = new(60, 56, 37);
        AlmanacMap map2 = new(56, 93, 4);

        var map = almanac.HumidityToLocationMap.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(map.Count, Is.EqualTo(2));
            Assert.That(map[0], Is.EqualTo(map1));
            Assert.That(map[1], Is.EqualTo(map2));
        });
    }

    [TestCase(98,50)]
    [TestCase(99,51)]
    public void seed_number_in_range_1(int seedNumber,int destination)
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        Assert.That(Almanac.Destination(seedNumber, almanac.SeedToSoilMap),Is.EqualTo(destination));
    }
    
    [TestCase(1)]
    [TestCase(10)]
    public void seed_number_not_in_any_range_gets_the_same_number(int seedNumber)
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        Assert.That(Almanac.Destination(seedNumber, almanac.SeedToSoilMap),Is.EqualTo(seedNumber));
    }
    
    [Test]
    public void seed_number_in_range_2([Range(52,97,1)]int seedNumber)
    {
        var destination = 50 + seedNumber - 48;
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        Assert.That(Almanac.Destination(seedNumber, almanac.SeedToSoilMap),Is.EqualTo(destination));
    }
    
    
    [Test]
    public void soil_number_in_range_3([Values(0,14,1)]int seedNumber)
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        var destination = seedNumber+39;
        Assert.That(Almanac.Destination(seedNumber, almanac.SoilToFertilizerMap),Is.EqualTo(destination));
    }
    
    [Test]
    public void soil_number_15_is_in_range_1_not_in_range_3()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        Assert.That(Almanac.Destination(15, almanac.SoilToFertilizerMap),Is.EqualTo(0));
    }

    [Test]
    public void Example_seed_to_soil_conversion()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        var seedToSoilMap = almanac.SeedToSoilMap.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(Almanac.Destination(79, seedToSoilMap), Is.EqualTo(81));
            Assert.That(Almanac.Destination(14, seedToSoilMap), Is.EqualTo(14));
            Assert.That(Almanac.Destination(55, seedToSoilMap), Is.EqualTo(57));
            Assert.That(Almanac.Destination(13, seedToSoilMap), Is.EqualTo(13));
        });
    }

    [Test]
    public void Chain_to_location()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        int seed = 79;
        var soil = Almanac.Destination(seed, almanac.SeedToSoilMap);
        Assert.That(soil,Is.EqualTo(81));
        var fertilizer = Almanac.Destination(soil, almanac.SoilToFertilizerMap);
        Assert.That(fertilizer,Is.EqualTo(81));
        var water = Almanac.Destination(fertilizer, almanac.FertilizerToWaterMap);
        Assert.That(water,Is.EqualTo(81));
        var light = Almanac.Destination(water, almanac.WaterToLightMap);
        Assert.That(light,Is.EqualTo(74));
        var temp  = Almanac.Destination(light, almanac.LightToTemperatureMap);
        Assert.That(temp,Is.EqualTo(78),"Temp should be 78");
        var humid  = Almanac.Destination(temp, almanac.TemperatureToHumidityMap);
        Assert.That(humid,Is.EqualTo(78),"Humidity should be 78");
        var location = Almanac.Destination(humid, almanac.HumidityToLocationMap);
        Assert.That(location,Is.EqualTo(82));
    }

    [TestCase(79,82)]
    [TestCase(14,43)]
    [TestCase(55,86)]
    [TestCase(13,35)]
    [TestCase(82,46,Description = "This is the part 2 example using ranges of seeds")]
    public void Chain_to_location_for_all_seeds(int seed, int location)
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        Assert.That(almanac.ChainOfMapping(seed),Is.EqualTo(location));
    }

    [Test]
    public void Lowest_location_for_puzzle_input()
    {
        var almanac = new Almanac(PuzzleInput.GetFile("day5.txt"));
        var location = almanac.Seeds.Select(seed => almanac.ChainOfMapping(seed)).Min();
        Assert.That(location,Is.EqualTo(88151870));
    }
    
    [Test]
    //[Ignore("Slow........")]
    public void Lowest_location_for_puzzle_input_with_a_range_of_seeds()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput), true);
        //var almanac = new Almanac(PuzzleInput.GetFile("day5.txt"),true);
        var location = almanac.Seeds.Select(seed => almanac.ChainOfMapping(seed)).Min();
        Assert.That(location,Is.EqualTo(46));
    }

    private const string ExamplePuzzleInput = @"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";
}

public class AlmanacMapShould
{
    [Test]
    public void Should_verify_if_an_item_is_in_the_range()
    {
        var almanacMap = new AlmanacMap(50, 98, 2);
        Assert.Multiple(() =>
        {
            Assert.That(almanacMap.ForItem(98), Is.True);
            Assert.That(almanacMap.ForItem(99), Is.True);
            Assert.That(almanacMap.ForItem(100), Is.False);
            Assert.That(almanacMap.ForItem(97), Is.False);
            Assert.That(almanacMap.ForItem(0), Is.False);
            Assert.That(almanacMap.ForItem(-10), Is.False);
        });
    }
}