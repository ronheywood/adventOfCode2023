using TestProject1.Helpers.Tests;

namespace TestProject1.Helpers;

public class Rope

{
    private readonly Dictionary<RopePullDirection,Action> _ropePullStrategies;
    public IList<string> UniqueRecordedTailMovements { get; set; }

    private int HeadLocationX { get; set; } = 0;
    private int HeadLocationY { get; set; } = 0;
    private int TailLocationX { get; set; } = 0;
    private int TailLocationY { get; set; } = 0;

    public Rope()
    {
        UniqueRecordedTailMovements = new List<string>() { "0,0" };
        _ropePullStrategies = new Dictionary<RopePullDirection, Action>
        {
            { RopePullDirection.North, PullNorth },
            { RopePullDirection.South, PullSouth },
            { RopePullDirection.East, PullEast },
            { RopePullDirection.West, PullWest }
        };
    }
    
    public void Move(RopePullDirection direction, int i)
    {
        var strategy = _ropePullStrategies[direction];
        
        while (i > 0)
        {
            strategy.Invoke();
            i--;
        }
        
    }

    private void PullWest()
    {
        HeadLocationX--;
        MoveTail();
    }

    private void PullSouth()
    {
        HeadLocationY--;
        MoveTail();
    }

    private void PullNorth()
    {
        HeadLocationY++;
        MoveTail();
    }

    private void PullEast()
    {
        HeadLocationX++;
        MoveTail();
    }

    private void MoveTail()
    {
        if (HeadLocationY - TailLocationY > 1) //moving north
        {
            TailLocationY++;
            TailLocationX = HeadLocationX;
        }

        if (HeadLocationY - TailLocationY < -1) //moving south
        {
            TailLocationY--;
            TailLocationX = HeadLocationX;
        }
        
        if (HeadLocationX - TailLocationX > 1) //moving east
        {
            TailLocationX++;
            TailLocationY = HeadLocationY;
        }
        
        if (HeadLocationX - TailLocationX < -1) //moving west
        {
            TailLocationX--;
            TailLocationY = HeadLocationY;
        }
        
        RecordTailLocation();
    }

    private void RecordTailLocation()
    {
        var item = $"{TailLocationX},{TailLocationY}";
        if (UniqueRecordedTailMovements.Contains(item)) return;
        UniqueRecordedTailMovements.Add(item);
    }

    public void ProcessPuzzleInput(IEnumerable<string> puzzleInput)
    {
        var translateDirections = new Dictionary<string,RopePullDirection>
        {
            {"R",RopePullDirection.East},
            {"U",RopePullDirection.North},
            {"L",RopePullDirection.West},
            {"D",RopePullDirection.South}
        };
        foreach (var line in puzzleInput)
        {
            var split = line.Split(" ");
            var direction = translateDirections[split[0]];
            var distance = int.Parse(split[1]);
            Move(direction, distance);
        }
    }
}