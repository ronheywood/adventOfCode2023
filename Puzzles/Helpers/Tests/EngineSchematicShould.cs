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
    
}