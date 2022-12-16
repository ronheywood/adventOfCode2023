namespace TestProject1.Helpers.Tests;

public class RopeTests
{
    [Test]
    public void Should_start_at_zero_z_zero_y()
    {
        var rope = new Rope();
        Assert.That(rope.UniqueRecordedTailMovements.Count, Is.EqualTo(1));
        Assert.That(rope.UniqueRecordedTailMovements.First, Is.EqualTo("0,0"));
    }

    [Test]
    public void If_head_moves_one_space_tail_stays_at_location()
    {
        var rope = new Rope();
        rope.Move(RopePullDirection.East, 1);
        Assert.That(rope.UniqueRecordedTailMovements.Count, Is.EqualTo(1));
        Assert.That(rope.UniqueRecordedTailMovements.First, Is.EqualTo("0,0"));
    }

    [TestCase(RopePullDirection.East,"1,0")]
    [TestCase(RopePullDirection.North,"0,1")]
    [TestCase(RopePullDirection.South,"0,-1")]
    [TestCase(RopePullDirection.West,"-1,0")]
    public void If_head_moves_two_spaces_tail_must_add_a_new_location(RopePullDirection direction, string expectedNewLocation)
    {
        var rope = new Rope();
        rope.Move(direction,2);
        Assert.That(rope.UniqueRecordedTailMovements.Count, Is.EqualTo(2));
        CollectionAssert.AreEqual(new[]{"0,0",expectedNewLocation},rope.UniqueRecordedTailMovements);
    }

    [Test]
    public void Should_not_duplicate_tail_locations()
    {
        var rope = new Rope();
        rope.Move(RopePullDirection.North,2);
        Assume.That(rope.UniqueRecordedTailMovements.Last, Is.EqualTo("0,1")); //tail moved to 0,1
        rope.Move(RopePullDirection.North,1);
        Assume.That(rope.UniqueRecordedTailMovements.Last, Is.EqualTo("0,2")); //tail moved to 0,2
        rope.Move(RopePullDirection.South,3);
        
        CollectionAssert.AreEqual(new[]{"0,0","0,1","0,2"},rope.UniqueRecordedTailMovements); //tail will move back to 0,1 but we already recorded this
    }

    [Test]
    public void Diagonal_forces_move_tail_east_and_north_when_resolved()
    {
        var rope = new Rope();
        rope.Move(RopePullDirection.East,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location unchanged
        
        rope.Move(RopePullDirection.North,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location still unchanged - diagonal forces
        
        rope.Move(RopePullDirection.North,1);
        CollectionAssert.AreEqual(new[]{"0,0","1,1"},rope.UniqueRecordedTailMovements); //head is now at 1,2 - diagonal forces are resolved on move
    }
    
    [Test]
    public void Diagonal_forces_move_tail_east_and_south_when_resolved()
    {
        var rope = new Rope();
        rope.Move(RopePullDirection.East,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location unchanged
        
        rope.Move(RopePullDirection.South,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location still unchanged - diagonal forces
        
        rope.Move(RopePullDirection.South,1);
        CollectionAssert.AreEqual(new[]{"0,0","1,-1"},rope.UniqueRecordedTailMovements); //head is now at 1,-2 - diagonal forces are resolved on move
    }
    
    [Test]
    public void Diagonal_forces_move_tail_west_and_north_when_resolved()
    {
        var rope = new Rope();
        rope.Move(RopePullDirection.West,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location unchanged
        
        rope.Move(RopePullDirection.North,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location still unchanged - diagonal forces
        
        rope.Move(RopePullDirection.North,1);
        CollectionAssert.AreEqual(new[]{"0,0","-1,1"},rope.UniqueRecordedTailMovements); //head is now at -1,2 - diagonal forces are resolved on move
    }
    
    [Test]
    public void Diagonal_forces_move_tail_west_and_south_when_resolved()
    {
        var rope = new Rope();
        rope.Move(RopePullDirection.West,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location unchanged
        
        rope.Move(RopePullDirection.South,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location still unchanged - diagonal forces
        
        rope.Move(RopePullDirection.South,1);
        CollectionAssert.AreEqual(new[]{"0,0","-1,-1"},rope.UniqueRecordedTailMovements); //head is now at -1,-2 - diagonal forces are resolved on move
    }
    
    [Test]
    public void Diagonal_forces_move_tail_north_and_east_when_resolved()
    {
        var rope = new Rope();
        rope.Move(RopePullDirection.North,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location unchanged
        
        rope.Move(RopePullDirection.East,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location still unchanged - diagonal forces
        
        rope.Move(RopePullDirection.East,1);
        CollectionAssert.AreEqual(new[]{"0,0","1,1"},rope.UniqueRecordedTailMovements); //head is now at 1,2 - diagonal forces are resolved on move
    }
    
    [Test]
    public void Diagonal_forces_move_tail_south_and_east_when_resolved()
    {
        var rope = new Rope();
        rope.Move(RopePullDirection.South,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location unchanged
        
        rope.Move(RopePullDirection.East,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location still unchanged - diagonal forces
        
        rope.Move(RopePullDirection.East,1);
        CollectionAssert.AreEqual(new[]{"0,0","1,-1"},rope.UniqueRecordedTailMovements); //head is now at 1,-2 - diagonal forces are resolved on move
    }
    
    [Test]
    public void Diagonal_forces_move_tail_north_and_west_when_resolved()
    {
        var rope = new Rope();
        rope.Move(RopePullDirection.North,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location unchanged
        
        rope.Move(RopePullDirection.West,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location still unchanged - diagonal forces
        
        rope.Move(RopePullDirection.West,1);
        CollectionAssert.AreEqual(new[]{"0,0","-1,1"},rope.UniqueRecordedTailMovements); //head is now at -2,1 - diagonal forces are resolved on move
    }
    
    [Test]
    public void Diagonal_forces_move_tail_south_and_west_when_resolved()
    {
        var rope = new Rope();
        rope.Move(RopePullDirection.South,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location unchanged
        
        rope.Move(RopePullDirection.West,1);
        CollectionAssert.AreEqual(new[]{"0,0"},rope.UniqueRecordedTailMovements); //starting location still unchanged - diagonal forces
        
        rope.Move(RopePullDirection.West,1);
        CollectionAssert.AreEqual(new[]{"0,0","-1,-1"},rope.UniqueRecordedTailMovements); //head is now at -2,-1 - diagonal forces are resolved on move
    }

    [Test]
    public void Should_convert_puzzle_input_to_movement()
    {
        const string specificationPuzzleInput = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";
        
        var puzzleInput = PuzzleInput.InputStringToArray(specificationPuzzleInput);
        var rope = new Rope();
        rope.ProcessPuzzleInput(puzzleInput);
        Assert.That(rope.UniqueRecordedTailMovements, Has.Count.EqualTo(13));
        //IEnumerable expectedTailLocations = new []{"0,0","1,0","2,0","3,0","4,1","4,2","4,3","3,4","2,4","3,2","3,1"};
        //CollectionAssert.AreEqual(expectedTailLocations,rope.UniqueRecordedTailMovements);
    }
}

public enum RopePullDirection
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}