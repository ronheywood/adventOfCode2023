using System.Collections;

namespace TestProject1.Helpers.Tests;

public class PipeMapShould
{
    private const string SimpleLoopMap = @"-----
|S-7.
.|.|.
.L-J.
.....";

    private const string ComplicatedLoopMap = @"..F7.
.FJ|.
SJ.L7
|F--J
LJ...";

    [Test]
    public void distance_is_zero_when_no_start_location()
    {
        var map = "";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(0));
    }

    [Test]
    public void distance_is_zero_when_start_location_is_the_only_location()
    {
        var map = "S";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(0));
    }

    [TestCaseSource(nameof(DisConnectedStartPipes))]
    public void distance_is_zero_when_start_location_is_not_connected_to_a_pipe(string map)
    {
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(0));
    }

    [TestCaseSource(nameof(IncompatiblePipes))]
    public void distance_is_zero_when_start_location_is_not_connected_to_a_compatible_pipe(string map)
    {
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(0));
    }

    [Test]
    public void start_pipe_is_always_connected_to_two_pipes()
    {
        //var map = @"-S-";
    }

    [Test]
    public async Task follows_connected_pipes_ignores_unconnected()
    {
        var pipeMap = new PipeMap(PuzzleInput.InputStringToArray(SimpleLoopMap));
        var distancePlot = pipeMap.DistancePlot();
        await Verify(distancePlot);
    }

    private static IEnumerable<object[]> IncompatiblePipes()
    {
        yield return new object[] { @"SF" };
        yield return new object[] { @"7S" };
        yield return new object[] { @"|S" };
        yield return new object[] { @"S|" };
        yield return new object[] { @"SL" };
        yield return new object[] { @"JS" };
        yield return new object[]
        {
            @"J
S"
        };
        yield return new object[]
        {
            @"L
S"
        };
        yield return new object[]
        {
            @"J
S"
        };
        yield return new object[]
        {
            @"S
F"
        };
        yield return new object[]
        {
            @"S
7"
        };
        yield return new object[]
        {
            @"S
-"
        };
        yield return new object[]
        {
            @"-
S"
        };
    }

    private static IEnumerable<object[]> DisConnectedStartPipes()
    {
        yield return new object[] { @"S.." };

        yield return new object[] { @"S..-" };

        yield return new object[] { @"S..|" };

        yield return new object[] { @"S..L" };

        yield return new object[] { @"S..J" };

        yield return new object[] { @"S..7" };

        yield return new object[] { @"S..F" };
    }

    [Test]
    public void distance_is_one_when_one_connected_horizontal_pipe()
    {
        var map = "S-";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(1));

        map = "-S";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(1));
    }

    [Test]
    public void distance_is_one_when_one_connected_vertical_pipe()
    {
        var map = @"|
S";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(1));

        map = @"S
|";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(1));
    }

    [Test]
    public void distance_is_one_when_one_connected_bend_pipe()
    {
        var map = @"S
L";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(1));
    }

    [Test]
    public void distance_is_one_when_one_connected_bend_pipe_west()
    {
        var map = @"S
J";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(1));
    }

    [Test]
    public void distance_is_one_when_one_connected_bend_pipe_east()
    {
        var map = @"S7";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(1));
    }

    [Test]
    public void distance_is_one_when_one_connected_bend_pipe_se()
    {
        var map = @"FS";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(1));
    }

    [TestCase("F")]
    [TestCase("7")]
    public void distance_is_one_when_one_connected_bend_pipe_north(string bendType)
    {
        var map = @$"{bendType}
S";
        Assert.That(new PipeMap(PuzzleInput.InputStringToArray(map)).Distance(), Is.EqualTo(1));
    }

    [TestCase("-", "-")]
    [TestCase("F", "7")]
    [TestCase("F", "-")]
    [TestCase("L", "J")]
    public void Identifies_valid_compass_orientations_horizontal_from_start(string west, string east)
    {
        var map = @$"{west}S{east}";
        var connectedToStart = new PipeMap(PuzzleInput.InputStringToArray(map)).ConnectedToStart().ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(connectedToStart, Has.Length.EqualTo(2));
            Assert.That(connectedToStart, Does.Contain(GridDirections.East));
            Assert.That(connectedToStart, Does.Contain(GridDirections.West));
        });
    }

    [TestCase("|", "|")]
    [TestCase("F", "|")]
    [TestCase("F", "J")]
    [TestCase("7", "L")]
    public void Identifies_valid_compass_orientations_vertical_from_start(string north, string south)
    {
        var map = @$".{north}.
.S.
.{south}.";
        var connectedToStart = new PipeMap(PuzzleInput.InputStringToArray(map)).ConnectedToStart().ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(connectedToStart, Does.Contain(GridDirections.North));
            Assert.That(connectedToStart, Does.Contain(GridDirections.South));
        });
    }

    [TestCase("-S-", GridDirections.West, GridDirections.East)]
    [TestCase("-S-", GridDirections.East, GridDirections.West)]
    [TestCase(@"|
S
|", GridDirections.North, GridDirections.South)]
    [TestCase(@"|
S
|", GridDirections.South, GridDirections.North)]
    public void identifies_valid_pipe_exit_from_entrance(string mapString, GridDirections entrance, GridDirections exit)
    {
        var map = new PipeMap(PuzzleInput.InputStringToArray(mapString));
        var start = map.StartLocation();
        Assert.That(map.Exit(start, entrance), Is.EqualTo(exit));
    }

    [TestCase("-", GridDirections.East, GridDirections.East)]
    [TestCase("-", GridDirections.West, GridDirections.West)]
    [TestCase("|", GridDirections.North, GridDirections.North)]
    [TestCase("|", GridDirections.South, GridDirections.South)]
    [TestCase("L", GridDirections.South, GridDirections.East)]
    [TestCase("L", GridDirections.West, GridDirections.North)]
    [TestCase("J", GridDirections.South, GridDirections.West)]
    [TestCase("J", GridDirections.East, GridDirections.North)]
    [TestCase("7", GridDirections.East, GridDirections.South)]
    [TestCase("7", GridDirections.North, GridDirections.West)]
    [TestCase("F", GridDirections.North, GridDirections.East)]
    [TestCase("F", GridDirections.West, GridDirections.South)]
    public void identifies_new_orientation_from_pipe_entrance(string pipeString, GridDirections entranceOrientation,
        GridDirections expectedOrientaion)
    {
        var orientation = PipeMap.Orientation(pipeString, entranceOrientation);
        Assert.That(orientation, Is.EqualTo(expectedOrientaion));
    }

    [Test]
    public void invalid_orientation_error_message()
    {
        var ex = Assert.Throws<Exception>(() => PipeMap.Orientation("-", GridDirections.North));
        var expectedMessage = "Failed to get new orientation from input -, North";
        Assert.That(ex?.Message, Is.EqualTo(expectedMessage));
    }

    [TestCaseSource(nameof(PipeRoute))]
    public void Get_next_location_from_a_pipe_and_an_orientation(Tuple<int, int> start, GridDirections orientation,
        Tuple<int, int, GridDirections> expected)
    {
        var pipeMap = new PipeMap(PuzzleInput.InputStringToArray(SimpleLoopMap));
        Assert.That(pipeMap.Next(start, orientation), Is.EqualTo(expected));
    }

    private static IEnumerable<object[]> PipeRoute()
    {
        yield return new object[]
        {
            new Tuple<int, int>(1, 1),
            GridDirections.East,
            new Tuple<int, int, GridDirections>(2, 1, GridDirections.East)
        };

        yield return new object[]
        {
            new Tuple<int, int>(2, 1),
            GridDirections.East,
            new Tuple<int, int, GridDirections>(3, 1, GridDirections.East)
        };

        yield return new object[]
        {
            new Tuple<int, int>(3, 1),
            GridDirections.East,
            new Tuple<int, int, GridDirections>(3, 2, GridDirections.South)
        };
        yield return new object[]
        {
            new Tuple<int, int>(3, 2),
            GridDirections.South,
            new Tuple<int, int, GridDirections>(3, 3, GridDirections.South)
        };
        yield return new object[]
        {
            new Tuple<int, int>(3, 3),
            GridDirections.South,
            new Tuple<int, int, GridDirections>(2, 3, GridDirections.West)
        };
        yield return new object[]
        {
            new Tuple<int, int>(2, 3),
            GridDirections.West,
            new Tuple<int, int, GridDirections>(1, 3, GridDirections.West)
        };
        yield return new object[]
        {
            new Tuple<int, int>(1, 3),
            GridDirections.West,
            new Tuple<int, int, GridDirections>(1, 2, GridDirections.North)
        };
        yield return new object[]
        {
            new Tuple<int, int>(1, 2),
            GridDirections.North,
            new Tuple<int, int, GridDirections>(1, 1, GridDirections.North)
        };
    }

    [Test]
    public void Follow_a_loop_to_start()
    {
        var map = new PipeMap(PuzzleInput.InputStringToArray(SimpleLoopMap));
        Assert.That(map.Route(), Is.EqualTo("S-7|J-L|"));
    }

    [Test]
    public void Follow_a_loop_in_example_puzzle()
    {
        var exampleMap = PuzzleInput.GetFile("day10.txt");
        var map = new PipeMap(exampleMap);
        Assert.That(map.Route(), Does.StartWith("S-J7LF---JF--7J-F")); //... 13780 chars
    }

    [Test]
    public void Should_calculate_max_distance_x_when_travelling_EAST()
    {
        var mapString = @"S---7
L---J";
        var map = new PipeMap(PuzzleInput.InputStringToArray(mapString));
        map.Route();
        Assert.That(map.MaxX, Is.EqualTo(4));
    }

    [Test]
    public void Should_calculate_max_distance_x_when_travelling_WEST()
    {
        var mapString = @"F---7
S---J";
        var map = new PipeMap(PuzzleInput.InputStringToArray(mapString));
        map.Route();
        Assert.That(map.MaxX, Is.EqualTo(4));
    }

    [Test]
    public void Should_calculate_max_distance_y_when_travelling_south()
    {
        var verticalMap = @"S7
||
||
LJ";
        var map = new PipeMap(PuzzleInput.InputStringToArray(verticalMap));
        map.Route();
        Assert.That(map.MaxY, Is.EqualTo(3));
        Assert.That(map.FarthestPoint(), Is.EqualTo(4),"4 steps to farthest point");
    }

    [Test]
    public void Should_calculate_max_distance_y_when_travelling_north()
    {
        var verticalMap = @"F7
||
||
SJ";
        var map = new PipeMap(PuzzleInput.InputStringToArray(verticalMap));
        map.Route();
        Assert.That(map.MaxY, Is.EqualTo(3));
        Assert.That(map.MaxX, Is.EqualTo(1));
        Assert.That(map.FarthestPoint(), Is.EqualTo(4));
    }

    [Test]
    public void max_distance_on_the_y_axis_when_traveling_north_from_start()
    {
        var map = new PipeMap(PuzzleInput.InputStringToArray(ComplicatedLoopMap));
        map.Route();
        Assert.That(map.MaxX, Is.EqualTo(4));
        Assert.That(map.MaxY, Is.EqualTo(2));
        //Assert.That(map.FarthestPoint(), Is.EqualTo(8));
    }

    [Test]
    public void Should_sum_the_max_distance_on_the_x_axis_and_max_distance_on_the_y_axis_to_get_the_farthest_point()
    {
        var map = new PipeMap(PuzzleInput.InputStringToArray(SimpleLoopMap));
        map.Route();
        Assert.That(map.MaxX, Is.EqualTo(2));
        Assert.That(map.MaxY, Is.EqualTo(2));
        Assert.That(map.FarthestPoint(), Is.EqualTo(4));
    }

    [Test]
    [Ignore("If we go up 1 need to go down 1 again to get back to start x")]
    public void sum_max_distance_by_stretching_the_y_axis_flat()
    {
        var map = new PipeMap(PuzzleInput.InputStringToArray(ComplicatedLoopMap));
        map.Route();
        Assert.That(map.MaxY, Is.EqualTo(2));
        Assert.That(map.MaxX, Is.EqualTo(4));
        Assert.That(map.FarthestPoint(), Is.EqualTo(8));
    }

    [Test]
    [Ignore("when end point is on x ")]
    public void sum_max_distance_by_stretching_the_y_axis_flat_when_odd_number_on_y()
    {
        var maxXOnSameAxis = @".F-7
SJ.|
L--J";
        
        var map = new PipeMap(PuzzleInput.InputStringToArray(maxXOnSameAxis));
        map.Route();
        
        Assert.That(map.MaxY, Is.EqualTo(1), "Max Y should be 1");
        Assert.That(map.MaxX, Is.EqualTo(3));
        Assert.That(map.FarthestPoint(), Is.EqualTo(5));
    }
    
    [Test]
    public void sum_max_distance_by_stretching_the_y_axis_flat_when_Y_descends()
    {
        var maxXOnUpperAxis = @"....
S--7
L-7|
..LJ";        
        var map = new PipeMap(PuzzleInput.InputStringToArray(maxXOnUpperAxis));
        map.Route();
        Assert.That(map.MaxY, Is.EqualTo(2));
        Assert.That(map.MaxX, Is.EqualTo(3));
        Assert.That(map.FarthestPoint(), Is.EqualTo(5));
    }
}