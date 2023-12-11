using System.Collections;

namespace TestProject1.Helpers;

public class Map : IEnumerable<MapLocation>
{
    private readonly MapLocation[] _locations;
    private readonly IEnumerator _directionsEnumerator;


    public Map(IEnumerable<string> puzzleLines)
    {
        var puzzleSegments = PuzzleInput.GetPuzzleSegments(puzzleLines, "").ToArray();
        var directions = puzzleSegments.First().First().ToCharArray();
        _directionsEnumerator = directions.GetEnumerator();
        _directionsEnumerator.MoveNext();
        
        var sequence = puzzleSegments.Last();
        _locations = sequence.Select(location => new MapLocation(location)).ToArray();
        var startIndex = StartIndex();
        Location  = _locations[startIndex];
    }

    private int StartIndex()
    {
        return Array.FindIndex(_locations, location => location.Key == "AAA");
    }

    public MapLocation? Location
    {
        get; private set;
    }

    private string Direction => _directionsEnumerator.Current?.ToString() ?? throw new Exception("Directions enumerator not reset");
    
    public IEnumerator<MapLocation> GetEnumerator()
    {
        var i = StartIndex();
        
        while (i < _locations.Length)
        {
            if (Location is null || Location.Key == "ZZZ")
            {
                Location = null;
                yield break;
            }
            
            Location = _locations[i];
            if (Direction == "R")
            {
                i = Array.FindIndex(_locations, location => location.Key == Location.Right);
            }
            else
            {
                i = Array.FindIndex(_locations, location => location.Key == Location.Left);
            }

            if (!_directionsEnumerator.MoveNext())
            {
                _directionsEnumerator.Reset();
                _directionsEnumerator.MoveNext();
            }
            yield return Location;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}