namespace TestProject1.Helpers.Tests;

public class PipeMapShould
{
    [Test]
    public void distance_is_zero_when_no_start_location()
    {
        var map = "";
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(0));
    }

    [Test]
    public void distance_is_zero_when_start_location_is_the_only_location()
    {
        var map = "S";
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(0));
    }

    [TestCaseSource(nameof(DisConnectedStartPipes))]
    public void distance_is_zero_when_start_location_is_not_connected_to_a_pipe(string map)
    {
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(0));
    }
    
    [TestCaseSource(nameof(IncompatiblePipes))]
    public void distance_is_zero_when_start_location_is_not_connected_to_a_compatible_pipe(string map)
    {
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(0));
    }

    [Test]
    public void start_pipe_is_always_connected_to_two_pipes()
    {
        //var map = @"-S-";
    }

    [Test]
    public async Task follows_connected_pipes_ignores_unconnected()
    {
        var map = @"-----
|S-7.
.|.|.
.L.J.
.....";
        var pipeMap = new PipeMap(map);
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
        yield return new object[] { @"J
S" };
        yield return new object[] { @"L
S" };
        yield return new object[] { @"J
S" };
        yield return new object[] { @"S
F" };
        yield return new object[] { @"S
7" };
        yield return new object[] { @"S
-" };   yield return new object[] { @"-
S" };
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
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(1));
        
        map = "-S";
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(1));
    }

    [Test]
    public void distance_is_one_when_one_connected_vertical_pipe()
    {
        var map = @"|
S";
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(1));
        
         map = @"S
|";
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(1));
    }

    [Test]
    public void distance_is_one_when_one_connected_bend_pipe()
    {
        var map = @"S
L";
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(1));
    }

    [Test]
    public void distance_is_one_when_one_connected_bend_pipe_west()
    {
        var map = @"S
J";
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(1));
    }

    [Test]
    public void distance_is_one_when_one_connected_bend_pipe_east()
    {
        var map = @"S7";
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(1));
    }
    
    [Test]
    public void distance_is_one_when_one_connected_bend_pipe_se()
    {
        var map = @"FS";
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(1));
    }
    
    [TestCase("F")]
    [TestCase("7")]
    public void distance_is_one_when_one_connected_bend_pipe_north(string bendType)
    {
        var map = @$"{bendType}
S";
        Assert.That(new PipeMap(map).Distance, Is.EqualTo(1));
    }
}

public class PipeMap
{
    private readonly string _map;
    private readonly PipePuzzleGrid _grid;
    private readonly GridCompass _gridCompass;

    public PipeMap(string map)
    {
        _map = map;
        _grid = new PipePuzzleGrid(PuzzleInput.InputStringToArray(map));
        _gridCompass = new GridCompass(_grid);
    }

    public string DistancePlot()
    {
        return _grid.DistancePlot();
    }

    public int Distance()
    {
        Tuple<int, int> start;
        try
        {
            start = _grid.StartLocation();
        }
        catch
        {
            return 0;
        }

        //Straights
        if (_gridCompass.WestNeighbor(start.Item1, start.Item2) == "-") return 1;
        if (_gridCompass.EastNeighbor(start.Item1, start.Item2) == "-") return 1;
        if (_gridCompass.NorthNeighbor(start.Item1, start.Item2) == "|") return 1;
        if (_gridCompass.SouthNeighbor(start.Item1, start.Item2) == "|") return 1;
        
        //Shoulders
        if (_gridCompass.SouthNeighbor(start.Item1, start.Item2) == "L") return 1;
        if (_gridCompass.SouthNeighbor(start.Item1, start.Item2) == "J") return 1;
        
        if (_gridCompass.NorthNeighbor(start.Item1, start.Item2) == "7") return 1;
        if (_gridCompass.NorthNeighbor(start.Item1, start.Item2) == "F") return 1
            ;
        if (_gridCompass.EastNeighbor(start.Item1, start.Item2) == "7") return 1;
        if (_gridCompass.WestNeighbor(start.Item1, start.Item2) == "F") return 1;
        
        return 0;
    }
}

public class PipePuzzleGrid : PuzzleGrid
{
    public PipePuzzleGrid(IEnumerable<string> map) : base(map)
    {
    }

    public Tuple<int, int> StartLocation()
    {
        for (var x = 0; x < GridWidth; x++)
        {
            for (var y = 0; y < GridHeight; y++)
            {
                if (GridCompass.GetItem(x, y) == "S") return new Tuple<int, int>(x, y);
            }
        }

        throw new Exception("StartLocation not found");
    }

    public string DistancePlot()
    {
        var response = string.Empty;
        var column = 0;
        var startFound = false;
        foreach (var item in Items)
        {
            if (item == "S") startFound = true;
            var itemString = ".";
            if (startFound)
            {
                itemString = (item == "S") ? "0" : item;                
            }
            response += itemString;
            if (column == GridWidth-1)
            {
                response += "\r\n";
                column = 0;    
            }
            else
            {
                column++;
            }
        }

        return response;
    }
}