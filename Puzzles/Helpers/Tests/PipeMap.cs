namespace TestProject1.Helpers.Tests;

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
        try
        {
            return Distance(_grid.StartLocation());
        }
        catch
        {
            return 0;
        }
    }
    
    public int Distance(Tuple<int, int> start)
    {
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

    public IEnumerable<GridDirections> ConnectedToStart()
    {
        return ValidExits(_grid.StartLocation());
    }

    private IEnumerable<GridDirections> ValidExits(Tuple<int, int> start)
    {
        var options = new List<GridDirections>();
        const string validNorth = "|7F";
        const string validEast = "-J7";
        const string validSouth = "|LJ";
        const string validWest = "-FL";

        var eastNeighbor = _gridCompass.EastNeighbor(start.Item1, start.Item2) ?? "X";
        var westNeighbor = _gridCompass.WestNeighbor(start.Item1, start.Item2) ?? "X";
        var northNeighbor = _gridCompass.NorthNeighbor(start.Item1, start.Item2) ?? "X";
        var southNeighbor = _gridCompass.SouthNeighbor(start.Item1, start.Item2) ?? "X";

        if (validEast.Contains(eastNeighbor)) options.Add(GridDirections.East);
        if (validWest.Contains(westNeighbor)) options.Add(GridDirections.West);
        if (validNorth.Contains(northNeighbor)) options.Add(GridDirections.North);
        if (validSouth.Contains(southNeighbor)) options.Add(GridDirections.South);

        return options;
    }

    public Tuple<int,int> StartLocation() => _grid.StartLocation();

    public GridDirections Exit(Tuple<int, int> start, GridDirections entrance)
    {
        return ValidExits(start).Single(exit => exit!=entrance);
    }

    public static GridDirections Orientation(string pipeString, GridDirections entranceOrientation)
    {
        return pipeString switch
        {
            "-" when entranceOrientation is GridDirections.East or GridDirections.West =>
                entranceOrientation,
            "|" when entranceOrientation is GridDirections.North or GridDirections.South => entranceOrientation,
            "7" when entranceOrientation == GridDirections.East => GridDirections.South,
            "7" when entranceOrientation == GridDirections.North => GridDirections.West,
            "L" when entranceOrientation == GridDirections.South => GridDirections.East,
            "L" when entranceOrientation == GridDirections.West => GridDirections.North,
            "J" when entranceOrientation == GridDirections.East => GridDirections.North,
            "J" when entranceOrientation == GridDirections.South => GridDirections.West,
            "F" when entranceOrientation == GridDirections.West => GridDirections.South,
            "F" when entranceOrientation == GridDirections.North => GridDirections.East,
            _ => throw new Exception($"Failed to get new orientation from input {pipeString}, {entranceOrientation}")
        };
    }

    public string Route()
    {
        var location = _grid.StartLocation();
        var orientation = ValidExits(location).First();
        var journey = "S";
        while (true)
        {
            var locationTuple = Next(location,orientation);
            var pipe = _gridCompass.GetItem(locationTuple.Item1,locationTuple.Item2) ?? ".";
            if (pipe == "S") break;
            
            location = new Tuple<int, int>(locationTuple.Item1, locationTuple.Item2);
            orientation = locationTuple.Item3;
            journey += pipe;
        }

        return journey;
    }

    public Tuple<int,int,GridDirections> Next(Tuple<int, int> location, GridDirections orientation)
    {
        var pipe = _gridCompass.GetItem(location.Item1,location.Item2) ?? "!";
        
        var exitOrientation = pipe == "S" ? ValidExits(location).First() 
            : Orientation(pipe, orientation);

        var nextX = location.Item1;
        var nextY = location.Item2;
        switch (exitOrientation)
        {
            case GridDirections.East:
            {
                nextX++;
                break;
            }
            case GridDirections.South:
            {
                nextY++;
                break;
            }
            case GridDirections.West:
            {
                nextX--;
                break;
            }
            case GridDirections.North:
            {
                nextY--;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }

        return new Tuple<int, int, GridDirections>(nextX, nextY, exitOrientation);
    }
}