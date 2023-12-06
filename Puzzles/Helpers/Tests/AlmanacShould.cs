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
        await Verify(almanac.SeedRanges);
    }
    
    [Test]
    public async Task Extract_many_ranges_of_seeds()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(@"seeds: 1 2 3 2 4 2"),true);
        await Verify(almanac.SeedRanges);
    }

    [Test]
    public async Task The_original_example_would_look_like_this_as_ranges()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(@"seeds: 13 2 55 1 79 1"),true);
        await Verify(almanac.SeedRanges);
    }

    [Test]
    public void Original_example_but_with_ranges_should_give_location_35()
    {
        // var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        // Assert.That(almanac.ChainOfMappingReversed(35), Is.EqualTo(13));
        // Assert.That(almanac.FirstLocationWithAValidSeed(), Is.EqualTo(35));
        //
        var inputStringToArray = PuzzleInput.InputStringToArray(ExamplePuzzleInput).ToArray();
        inputStringToArray[0] = "seeds: 13 2 55 1 79 1";
        
        var almanacRange = new Almanac(inputStringToArray,true);
        Assert.That(almanacRange.ChainOfMappingReversed(35), Is.EqualTo(13));
        Assert.That(almanacRange.FirstLocationWithAValidSeed(), Is.EqualTo(35));
    }

    [Test]
    public void The_list_of_valid_locations_is_limited()
    {
        var almanac = new Almanac(PuzzleInput.InputStringToArray(ExamplePuzzleInput));
        var bigAlmanac = new Almanac(PuzzleInput.GetFile("day5.txt"));
        Assert.Multiple(() =>
        {
            Assert.That(almanac.NumberOfValidSeedLocations, Is.EqualTo(41));
            Assert.That(almanac.MinPossibleSeedLocation, Is.EqualTo(56));
            Assert.That(almanac.MaxPossibleSeedLocation, Is.EqualTo(97));
            Assert.That(bigAlmanac.NumberOfValidSeedLocations,Is.EqualTo(4178076444));
        });
    }

    [Test]
    public void we_can_skip_gaps_in_ranges()
    {
        var exampleLocations = @"seeds: 79 14 55 13
humidity-to-location map:
0 56 10
50 93 10";
        var almanac = new Almanac(PuzzleInput.InputStringToArray(exampleLocations));
        Assert.That(almanac.HumidityToLocationMap.Count(), Is.EqualTo(2));
        CollectionAssert.AreEqual(new long[]{0,1,2,3,4,5,6,7,8,9},almanac.HumidityToLocationMap.First().ValidDestinations());
        CollectionAssert.AreEqual(new long[]{50,51,52,53,54,55,56,57,58,59},almanac.HumidityToLocationMap.Last().ValidDestinations());
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
    public void Lowest_location_for_puzzle_input_with_a_range_of_seeds()
    {
        var inputStringToArray = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var almanac = new Almanac(inputStringToArray, true);
        var location = almanac.FirstLocationWithAValidSeed();
        Assert.That(location,Is.EqualTo(46));
    }
    
    [Test]
    [Ignore("Still Too slow, let's put some chaos into it")]
    public void Lowest_location_for_big_puzzle_input_with_a_range_of_seeds()
    {
        var almanac = new Almanac(PuzzleInput.GetFile("day5.txt"), true);
        var location = almanac.FirstLocationWithAValidSeed();
        Assert.That(location,Is.EqualTo(46));
    }

    [Test]
    [Ignore("I'm sure that 2008786 is the lowest location with a seed but it's apparently too high")]
    public void Random_location_tests_to_find_a_better_range_of_locations()
    {
        var almanac = new Almanac(PuzzleInput.GetFile("day5.txt"), true);
        var randomizer = new Random();
        long lower = 2008786;
        Assert.That(almanac.MinPossibleSeedLocation, Is.LessThan(2008786));
        Assert.That(almanac.LocationHasSeed(lower),Is.True);
        
        //now work back from the lower
        for (var i = 0; i <= 2008786; i++)
        {
            if(almanac.LocationHasSeed(i))
                throw new Exception($"{i} is a valid location");
        }
        
        Assert.That(lower,Is.LessThan(2008786));
    }
    
    [Test]
    [Ignore("This suggests that range 6 comes up with smaller locations")]
    public void Guess_at_it([Range(0,9)] int testSet)
    {
        var almanac = new Almanac(PuzzleInput.GetFile("day5.txt"),true);

        var ranges = almanac.SeedRanges.ToArray();
        //await Verify(ranges);
        
        //find the range with the smallest gap
        var range = ranges[testSet];

        //A few random Picks from that range
        List<long> testSeeds = new();
        for (var i = 0; i < 1000; i++)
        {
            var random = new Random();
            testSeeds.Add(random.NextInt64(range.Item1, range.Item2));
        }
        
        var location = testSeeds.Select(seed => almanac.ChainOfMapping(seed)).Min();
        Assert.That(location,Is.EqualTo(0));
    }

    [Test]
    [Ignore("still slow")]
    public void Looks_like_range_6()
    {
        
        var almanac = new Almanac(PuzzleInput.GetFile("day5.txt"),true);

        var ranges = almanac.SeedRanges.ToArray();
        var range = ranges[6];
        List<long> testSeeds = new();
        
        for (var i = range.Item1; i < range.Item2; i++)
        {
            testSeeds.Add(i);
        }
        
        var location = testSeeds.Select(seed => almanac.ChainOfMapping(seed)).Min();
        
        Assert.That(location,Is.EqualTo(0));
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

    [Test]
    public void Should_verify_if_a_destination_was_in_the_range()
    {
        var almanacMap = new AlmanacMap(60, 56, 37);
        Assert.Multiple(() =>
        {
            Assert.That(almanacMap.ForDestination(59), Is.False);
            Assert.That(almanacMap.ForDestination(60), Is.True);
            Assert.That(almanacMap.ForDestination(61), Is.True);
            Assert.That(almanacMap.ForDestination(97), Is.True);
            Assert.That(almanacMap.ForDestination(98), Is.False);
        });
    }

    [Test]
    public void Provide_the_source_from_a_destination()
    {
        //Valid destinations for this map run from 60 up to 97
        //And valid sources start from 56 to 93
        var almanacMap = new AlmanacMap(60, 56, 37);
        
        //So Destination 60 must have been derived from source: 56 + (60 - 60) -> 56
        //Destination 82 must have been derived from source 56 + (82 - 60)
        //Which means source 56 + offset 22 is 78
        Assert.Multiple(() =>
        {
            Assert.That(almanacMap.Source(60), Is.EqualTo(56));
            Assert.That(almanacMap.Destination(56), Is.EqualTo(60));
            
            Assert.That(almanacMap.Source(61), Is.EqualTo(57));
            Assert.That(almanacMap.Destination(57), Is.EqualTo(61));
            
            Assert.That(almanacMap.Source(82), Is.EqualTo(78));
            Assert.That(almanacMap.Destination(78), Is.EqualTo(82));
            
            Assert.That(almanacMap.Source(93), Is.EqualTo(89));
            Assert.That(almanacMap.Destination(89), Is.EqualTo(93));
            
            Assert.That(almanacMap.Source(98), Is.EqualTo(98));
        });
    }
}