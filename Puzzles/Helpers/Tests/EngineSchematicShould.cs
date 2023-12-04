namespace TestProject1.Helpers.Tests;

public class EngineSchematicShould
{
    private const string ExampleSchematic = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";


    [Test]
    public void Should_convert_input_to_puzzle_grid()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExampleSchematic);
        var puzzleGrid = new GridCompass(new PuzzleGrid(puzzleLines));
        Assert.Multiple(() =>
        {
            Assert.That(puzzleGrid.GetItem(0, 0), Is.EqualTo("4"));
            Assert.That(puzzleGrid.GetItem(0, 1), Is.EqualTo("."));
            Assert.That(puzzleGrid.GetItem(3, 1), Is.EqualTo("*"));
            Assert.That(puzzleGrid.WestNeighbor(3, 4), Is.EqualTo("7"));
            Assert.That(puzzleGrid.SouthWestNeighbor(5, 5), Is.EqualTo("2"));
        });
    }

    [Test]
    public async Task Engine_schematic_grid_finds_schematic_symbols_locations()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExampleSchematic);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var parts = puzzleGrid.FindParts();
        await Verify(parts);
    }

    [Test]
    public void Identifies_many_whole_numbers_on_a_long_line()
    {
        const string longLine = @"123..456..7.8910.123456789";
        var puzzleLines = PuzzleInput.InputStringToArray(longLine);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var number = puzzleGrid.FindNumbers();
        
        CollectionAssert.AreEqual(new[]{123,456,7,8910,123456789},number.Select(tuple => tuple.Item3));
    }
    
    [Test]
    public async Task Identifies_all_whole_numbers()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExampleSchematic);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var number = puzzleGrid.FindNumbers();
        await Verify(number);
    }

    [Test]
    public void Filters_numbers_not_connected_to_a_part()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExampleSchematic);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var number = puzzleGrid.FindNumbers();

        var filteredNumbers = puzzleGrid.FilterPartNumbers(number).ToList();
        CollectionAssert.AreEqual(new[]{467,35,633,617,592,755,664,598},filteredNumbers);
        Assert.That(filteredNumbers.Sum(),Is.EqualTo(4361));
    }

    [Test]
    public void Should_sum_all_parts_from_puzzle_input()
    {
        var puzzleGrid = new EngineSchematicGrid(PuzzleInput.GetFile("day3.txt"));
        var number = puzzleGrid.FindNumbers();

        var filteredNumbers = puzzleGrid.FilterPartNumbers(number).ToList();
        
        Assert.That(filteredNumbers.Sum(),Is.EqualTo(537832));
    }

    [Test]
    public void Should_find_all_numbers_attached_to_gear_symbols()
    {
        var puzzleLines = PuzzleInput.GetFile("day3.txt");
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var parts = puzzleGrid.FindGearSymbols().ToArray();
        var numbers = puzzleGrid.FindNumbers();
        var gear = parts[0];
        var numbersAttachedToThisGear = puzzleGrid.NumbersAttachedToThisGear(numbers, gear).ToArray();
        CollectionAssert.AreEqual(new[] {180,923},numbersAttachedToThisGear);
    }

    [Test]
    public void Should_filter_gear_symbols()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExampleSchematic);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var ratios = puzzleGrid.FindGearRatios();
        Assert.That( ratios.Sum(), Is.EqualTo(467835));
    }
    
    [Test]
    //[Ignore("60725088 is too low")]
    public void Sum_of_ratios_in_puzzle_input()
    {
        var puzzleLines = PuzzleInput.GetFile("day3.txt");
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var ratios = puzzleGrid.FindGearRatios();
        Assert.That( ratios.Sum(), Is.EqualTo(81939900));
    }
    
    private const string AGearBetweenTwoNumbers = "1*2";
    private const string AGearNotBetweenTwoNumbers = "3*.4";
    [TestCase(AGearBetweenTwoNumbers,2)]
    [TestCase(AGearNotBetweenTwoNumbers,0)]
    public void Should_filter_gear_symbols_with_2_adjacent_integers(string input, int expected)
    {
        var puzzleLines = PuzzleInput.InputStringToArray(input);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var ratios = puzzleGrid.FindGearRatios().Sum();
        Assert.That( ratios, Is.EqualTo(expected));
    }
    
    [Test]
    public void Should_filter_gear_symbols_with_integers_north_and_south()
    {
        var input = @"1
*
2";
        var puzzleLines = PuzzleInput.InputStringToArray(input);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var ratios = puzzleGrid.FindGearRatios().Sum();
        Assert.That( ratios, Is.EqualTo(2));
    }
    
    [Test]
    public void Should_filter_gear_symbols_with_integers_northeast_and_southwest()
    {
        var input = @"1..
.*.
..2";
        var puzzleLines = PuzzleInput.InputStringToArray(input);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var ratios = puzzleGrid.FindGearRatios().Sum();
        Assert.That( ratios, Is.EqualTo(2));
        
        
        var input2 = @"..1
.*.
2..";
        var puzzleLines2 = PuzzleInput.InputStringToArray(input2);
        var puzzleGrid2 = new EngineSchematicGrid(puzzleLines2);
        var ratios2 = puzzleGrid2.FindGearRatios().Sum();
        Assert.That( ratios2, Is.EqualTo(2),"1 Northeast, 2 southwest");
    }

    [Test]
    public void Numbers_attached_north_east_and_ending_south()
    {
        var input = @"180.
...*
.923";
        var puzzleLines2 = PuzzleInput.InputStringToArray(input);
        var puzzleGrid2 = new EngineSchematicGrid(puzzleLines2);
        var gearRatios = puzzleGrid2.FindGearRatios();
        var expected = 180*923;
        CollectionAssert.AreEqual(new[] { expected },gearRatios);
    }
    
    [Test]
    public void Numbers_ending_northwest_and_southwest()
    {
        var input = @"180.
...*
923.";
        var puzzleLines2 = PuzzleInput.InputStringToArray(input);
        var puzzleGrid2 = new EngineSchematicGrid(puzzleLines2);
        var gearRatios = puzzleGrid2.FindGearRatios();
        var expected = 180*923;
        CollectionAssert.AreEqual(new[] { expected },gearRatios);
    }
    
    [Test]
    public void Numbers_ending_north_and_south()
    {
        var input = @"
0123
.180
...*
.923";
        var puzzleLines2 = PuzzleInput.InputStringToArray(input);
        var puzzleGrid2 = new EngineSchematicGrid(puzzleLines2);
        var gearRatios = puzzleGrid2.FindGearRatios();
        var expected = 180*923;
        CollectionAssert.AreEqual(new[] { expected },gearRatios);
    }

    [Test]
    public void Specific_match_1()
    {
        var input = @"467..114..
...*......
..35..633.";
        var puzzleLines = PuzzleInput.InputStringToArray(input);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var ratios = puzzleGrid.FindGearRatios().Sum();
        Assert.That( ratios, Is.EqualTo(16345)); // 437 * 35
        
    }
    
    [Test]
    public void Specific_match_2()
    {
        var input = @"......755.
...$.*....
.664.598..";
        var puzzleLines = PuzzleInput.InputStringToArray(input);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var ratios = puzzleGrid.FindGearRatios().Sum();
        Assert.That( ratios, Is.EqualTo(451490)); // 755 * 598
        
    }

    [Test]
    public void Simplified_match_2()
    {
        var input = @"......755.
.....*....
.....598..";
        var puzzleLines = PuzzleInput.InputStringToArray(input);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var ratios = puzzleGrid.FindGearRatios().Sum();
        Assert.That( ratios, Is.EqualTo(451490));
    }
    
    [Test]
    public void ignore_number_not_attached_to_a_gear()
    {
        var input = @"......755.
...$.*....
.664..598..";
        var puzzleLines = PuzzleInput.InputStringToArray(input);
        var puzzleGrid = new EngineSchematicGrid(puzzleLines);
        var ratios = puzzleGrid.FindGearRatios().Sum();
        Assert.That( ratios, Is.EqualTo(451490));
    }
}