using System.Collections;
using NUnit.Framework.Constraints;

namespace TestProject1.Helpers.Tests;

public class CubeGameShould
{
    private IEnumerable<Tuple<string,string>> _puzzleTuple;

    public const string ExamplePuzzleInput = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";
    
    [SetUp]
    public void SetUp()
    {
        var puzzleInput = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var puzzleTuple = PuzzleInput.GetPuzzlePairs(puzzleInput, ":");
        _puzzleTuple = puzzleTuple;
    }
    
    [Test]
    public async Task Extract_game_and_results_from_puzzle_input()
    {
        await Verify(_puzzleTuple);
    }

    [TestCase("8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red")]
    [TestCase("1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red")]
    [TestCase("1 green, 6 blue; 3 green; 3 green, 15 blue, 13 red")]
    public void Should_show_false_if_game_shows_more_than_12_red_cubes(string moreThan12Red)
    {
        var game = new Tuple<string, string>("Game 1", moreThan12Red);
        Assert.That(CubeGame.CheckCubes(game), Is.False);
    }
    
    [TestCase("15 blue; 5 blue; 5 green, 1 red")]
    [TestCase("1 green, 3 red, 15 blue; 3 green, 6 red; 3 green, 15 blue, 1 red")]
    [TestCase("1 green, 6 blue; 3 green; 3 green, 15 blue")]
    public void Should_show_false_if_game_shows_more_than_14_blue_cubes(string moreThan12Red)
    {
        var game = new Tuple<string, string>("Game 1", moreThan12Red);
        Assert.That(CubeGame.CheckCubes(game), Is.False);
    }
    
    [TestCase("8 green, 6 blue, 2 red; 5 blue, 4 red, 14 green; 5 green, 1 red")]
    [TestCase("1 green, 3 red, 6 blue; 3 green, 6 red; 20 green, 15 blue, 1 red")]
    [TestCase("1 green, 6 blue; 3 green, 1 blue; 20 green, 15 blue, 1 red")]
    public void Should_show_false_if_game_shows_more_than_13_green_cubes(string moreThan13Green)
    {
        var game = new Tuple<string, string>("Game 1", moreThan13Green);
        Assert.That(CubeGame.CheckCubes(game), Is.False);
    }
    
    [TestCase("8 green, 6 blue, 2 red; 5 blue, 4 red, 13 green; 5 green, 1 red")]
    [TestCase("1 green, 3 red, 6 blue; 3 green, 6 red; 2 green, 1 blue, 1 red")]
    [TestCase("6 blue; 1 blue; 1 blue, 1 red")]
    public void Should_show_true_if_game_shows_up_to_13_green_cubes(string moreThan13Green)
    {
        var game = new Tuple<string, string>("Game 1", moreThan13Green);
        Assert.That(CubeGame.CheckCubes(game), Is.True);
    }

    [TestCase(" 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green")]
    [TestCase(" 3 blue, 4 red; 12 red, 2 green, 6 blue; 2 green")]
    [TestCase(" 3 blue; 2 green, 6 blue; 2 green")]
    public void Should_show_true_if_game_shows_up_to_12_red_cubes(string upto12Red)
    {
        var game = new Tuple<string, string>("Game 1", upto12Red);
        Assert.That(CubeGame.CheckCubes(game), Is.True);
    }

    [Test]
    public async Task Identifies_games_where_cube_check_passes()
    {
        var games = CubeGame.WinningGames(_puzzleTuple);
        await Verify(games);
    }

    [TestCase("3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",4,2,6)]
    [TestCase("1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",1,3,4)]
    [TestCase("8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",20,13,6)]
    [TestCase("1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",14,3,15)]
    [TestCase("6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",6,3,2)]
    public void Should_identify_the_minimum_number_of_cubes_required_for_a_valid_game(string reveals, int expectedRed, int expectedGreen, int expectedBlue)
    {
        var game = new Tuple<string, string>("Game 1", reveals);
        
        var red = CubeGame.MostCubes(game, "red");
        var green = CubeGame.MostCubes(game, "green");
        var blue = CubeGame.MostCubes(game, "blue");
        Assert.Multiple(() =>
        {
            Assert.That(red, Is.EqualTo(expectedRed), "Red should be 4");
            Assert.That(green, Is.EqualTo(expectedGreen),"Green should be 2");
            Assert.That(blue, Is.EqualTo(expectedBlue), "Blue should be 6");
        });
    }

    [TestCase("3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",48)]
    [TestCase("1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",12)]
    [TestCase("8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",1560)]
    [TestCase("1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",630)]
    [TestCase("6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",36)]
    public void Sum_of_the_minimum_number_of_cubes_gives_the_power_of_a_game(string reveals, int expectedSum)
    {
        var game = new Tuple<string, string>("Game 1", reveals);
        Assert.That(CubeGame.SumPower(game),Is.EqualTo(expectedSum));
    }

    [Test]
    public void Sum_the_id_of_games()
    {
        var sumOfGameId = CubeGame.SumGameId(_puzzleTuple);
        Assert.That(sumOfGameId,Is.EqualTo(15));
        
        var sumOfValidGameIds = CubeGame.SumGameId(CubeGame.WinningGames(_puzzleTuple));
        Assert.That(sumOfValidGameIds,Is.EqualTo(8));
    }

    [Test]
    public void Sum_the_id_of_games_from_puzzle_input()
    {
        var puzzleInput = PuzzleInput.GetFile("day2.txt");
        var puzzleTuples = PuzzleInput.GetPuzzlePairs(puzzleInput, ":");
        var sumOfValidGameIds = CubeGame.SumGameId(CubeGame.WinningGames(puzzleTuples));
        Assert.That(sumOfValidGameIds, Is.EqualTo(2162));
    }
    
    [Test]
    public void Sum_the_power_of_all_games_from_puzzle_input()
    {
        var puzzleInput = PuzzleInput.GetFile("day2.txt");
        var puzzleTuples = PuzzleInput.GetPuzzlePairs(puzzleInput, ":");
        var sum = puzzleTuples.Sum(CubeGame.SumPower);

        Assert.That(sum,Is.EqualTo(72513));
    }
}