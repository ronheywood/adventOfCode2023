namespace TestProject1.Helpers.Tests;

public class MapShould
{
    private const string ExamplePuzzleTwoSteps = @"RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)";


    private const string ExamplePuzzleSixSteps = @"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)";
    
    [Test]
    public void Puzzle_input_segments_as_directions()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleTwoSteps);
        var puzzleSegments = PuzzleInput.GetPuzzleSegments(puzzleLines, "").ToArray();
        
        var directions = puzzleSegments.First().First();
        Assert.That(directions,Is.EqualTo("RL"));
        
        var map = puzzleSegments.Last().ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(map.First(), Does.StartWith("AAA"));
            Assert.That(map.Last(), Does.StartWith("ZZZ"));
        });
    }

    [Test]
     public void expose_current_location()
     {
         var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleTwoSteps);
         var map = new Map(puzzleLines);
         Assert.That(map.Location,Is.EqualTo(new MapLocation("AAA = (BBB, CCC)")));
     }
     
     [Test]
     public void Should_enumerate_location()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleTwoSteps);
         var map = new Map(puzzleLines);
         using var mapEnumerator = map.GetEnumerator();
         Assert.That(mapEnumerator.Current,Is.Null);
         mapEnumerator.MoveNext();
        Assert.Multiple(() =>
        {
            Assert.That(mapEnumerator.Current.Key, Is.EqualTo("AAA"));
            Assert.That(map.Location?.Key, Is.EqualTo("AAA"));
        });
    }

    [Test]
     public void Should_move_to_location_right()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleTwoSteps);
         var map = new Map(puzzleLines);
         using var mapEnumerator = map.GetEnumerator();
         Assert.That(mapEnumerator.Current,Is.Null);
         mapEnumerator.MoveNext();
         mapEnumerator.MoveNext();
         Assert.That(map.Location?.Key, Is.EqualTo("CCC"));
    }

     [Test]
     public void Should_move_to_location_left()
     {
         var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleTwoSteps);
         var map = new Map(puzzleLines);
         using var mapEnumerator = map.GetEnumerator();
         Assert.That(mapEnumerator.Current,Is.Null);
         mapEnumerator.MoveNext();
         mapEnumerator.MoveNext();
         mapEnumerator.MoveNext();
         Assert.That(map.Location?.Key, Is.EqualTo("ZZZ"));
     }

     [Test]
     public void Should_stop_when_final_location_is_reached()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleTwoSteps).ToArray();
         var map = new Map(puzzleLines);
         using var mapEnumerator = map.GetEnumerator();
         Assert.That(mapEnumerator.Current,Is.Null);
         mapEnumerator.MoveNext();
         mapEnumerator.MoveNext();
         mapEnumerator.MoveNext();
        Assert.Multiple(() =>
        {
            Assert.That(map.Location?.Key, Is.EqualTo("ZZZ"));
            Assert.That(mapEnumerator.MoveNext(), Is.False);
            Assert.That(map.Location, Is.Null);
        });
        
        map = new Map(puzzleLines);
        CollectionAssert.AreEqual(new[]{"AAA","CCC","ZZZ"},map.ToArray().Select(l => l.Key));
    }

     [Test]
     public void to_array_method()
     {
         var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleTwoSteps).ToArray();
         var map = new Map(puzzleLines);
         CollectionAssert.AreEqual(new[]{"AAA","CCC","ZZZ"},map.ToArray().Select(l => l.Key));    
     }
     
     [Test]
     public void to_array_method_with_more_steps()
     {
         var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleSixSteps).ToArray();
         var map = new Map(puzzleLines);
         var steps = map.ToArray().Select(l => l.Key);
         CollectionAssert.AreEqual(new[]{"AAA","BBB","AAA","BBB","AAA","BBB","ZZZ"},steps);
         Assert.That(steps.Count()-1,Is.EqualTo(6));
     }

     [Test]
     public void Should_set_location_to_AAA_when_puzzle_input_does_not_start_with_AAA()
     {
         string puzzleInputStartingFromAnotherLocation = @"RL

DBQ = (RTP, ZZZ)
NFX = (DBQ, PLG)
VBK = (BRV, DKG)
AAA = (DBQ, NFX)
ZZZ = (ZZZ, ZZZ)
";
         var puzzleLines = PuzzleInput.InputStringToArray(puzzleInputStartingFromAnotherLocation).ToArray();
         var map = new Map(puzzleLines);
         Assert.That(map.Location.Key,Is.EqualTo("AAA"));
         Assert.That(map.Location.Key,Is.EqualTo("AAA"));
         
         using var mapEnumerator = map.GetEnumerator();
         Assert.That(mapEnumerator.Current,Is.Null);
         mapEnumerator.MoveNext();
         Assert.That(map.Location.Key,Is.EqualTo("AAA"));
         mapEnumerator.MoveNext();
         Assert.That(map.Location.Key,Is.EqualTo("NFX"));
         
         map = new Map(puzzleLines);
         CollectionAssert.AreEqual(new[]{"AAA","NFX","DBQ","ZZZ"},map.ToArray().Select(l => l.Key));
     }

     [Test]
     public void to_array_method_with_puzzle_input()
     {
         var puzzleLines = PuzzleInput.GetFile("day8.txt");
         var map = new Map(puzzleLines);
         var steps = map.ToArray().Select(l => l.Key);
         Assert.That(steps.Count()-1,Is.EqualTo(12737));
     }
}