namespace TestProject1.Helpers.Tests;

public class CraneStackTest
{
    private const string PuzzleStackString = @"                  [Q]     [P] [P]
                [G] [V] [S] [Z] [F]
            [W] [V] [F] [Z] [W] [Q]
        [V] [T] [N] [J] [W] [B] [W]
    [Z] [L] [V] [B] [C] [R] [N] [M]
[C] [W] [R] [H] [H] [P] [T] [M] [B]
[Q] [Q] [M] [Z] [Z] [N] [G] [G] [J]
[B] [R] [B] [C] [D] [H] [D] [C] [N]
 1   2   3   4   5   6   7   8   9 

"; //instructions for moving crates appear after the blank line

    private const string StackOneRowOneColumn = @"[A]
 1 ";

    private const string StackTwoRowOneColumn = @"[A]
[B]
 1 ";

    private const string StackThreeRowOneColumn = @"[C]
[B]
[A]
 1 ";

    private const string SpecExampleStackString = @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 
";

    [Test]
    public void Should_setup_one_stack_for_each_column()
    {
        var lines = PuzzleInput.InputStringToArray(PuzzleStackString);

        var craneStack = new CraneStack(lines);
        var result = craneStack.Stacks;
        Assert.That(result.Count(), Is.EqualTo(9));
    }

    [TestCaseSource(nameof(StackConfigurations))]
    public void Should_populate_stack_one_with_column_data(string stackString, int expectedStackCount,
        IEnumerable<string> expectedStack)
    {
        var craneStack = new CraneStack(PuzzleInput.InputStringToArray(stackString));
        Assert.That(craneStack.Stacks.Count(), Is.EqualTo(1));
        var stack = craneStack.Stacks.First();
        Assert.That(stack.Count, Is.EqualTo(expectedStackCount));
        CollectionAssert.AreEqual(expectedStack, stack.ToArray());
    }

    [Test]
    public void Should_populate_stacks_with_column_data()
    {
        var craneStack = new CraneStack(PuzzleInput.InputStringToArray(SpecExampleStackString));
        Assert.That(craneStack.Stacks.Count(), Is.EqualTo(3));
        var column1 = craneStack.Stacks.First().ToArray();
        var column2 = craneStack.Stacks.Skip(1).First().ToArray();
        var column3 = craneStack.Stacks.Skip(2).First().ToArray();
        CollectionAssert.AreEqual(new[] { "N", "Z" }, column1, "Column 1 not as expected");
        CollectionAssert.AreEqual(new[] { "D", "C", "M" }, column2, "Column 2 not as expected");
        CollectionAssert.AreEqual(new[] { "P" }, column3, "Column 3 not as expected");
    }

    [Test]
    public void Should_gather_instructions_to_process_in_order()
    {
        var stackStringWithInstructions = SpecExampleStackString + "\r\n" + SpecExampleInstructions;
        var craneStack = new CraneStack(PuzzleInput.InputStringToArray(stackStringWithInstructions));

        Assert.That(craneStack.Instructions, Is.InstanceOf<IEnumerable<CraneMoveInstruction>>());
        Assert.That(craneStack.Instructions.Count(), Is.EqualTo(4));
        Assert.That(craneStack.Instructions.First().MoveHowMany, Is.EqualTo(1));
        Assert.That(craneStack.Instructions.First().TakeFromStack, Is.EqualTo(2));
        Assert.That(craneStack.Instructions.First().PushToStack, Is.EqualTo(1));
    }

    [Test]
    public void Should_change_state_on_instruction()
    {
        var craneStack = new CraneStack(PuzzleInput.InputStringToArray(SpecExampleStackString));

        CraneMoveInstruction instruction = new (){MoveHowMany = 1,TakeFromStack = 2,PushToStack = 1};
        craneStack.ProcessInstruction(instruction);
        CollectionAssert.AreEqual(new[]{"D","N","Z"},craneStack.Stacks.First().ToArray());
    }

    [Test]
    public void Should_move_many_crates_one_at_a_time()
    {
        var craneStack = new CraneStack(PuzzleInput.InputStringToArray(SpecExampleStackString));

        CraneMoveInstruction instruction = new (){MoveHowMany = 2,TakeFromStack = 1,PushToStack = 3};
        craneStack.ProcessInstruction(instruction);
        Assert.That(craneStack.Stacks.First().ToArray(),Is.Empty);
        CollectionAssert.AreEqual(new[]{"Z","N","P"},craneStack.Stacks.Skip(2).First().ToArray());
    }

    [Test]
    public void Should_move_many_crates_in_a_single_move_when_configured()
    {
        var craneStack = new CraneStack(PuzzleInput.InputStringToArray(SpecExampleStackString));
        craneStack.CanMoveMany = true;
        CraneMoveInstruction instruction = new (){MoveHowMany = 2,TakeFromStack = 1,PushToStack = 3};
        craneStack.ProcessInstruction(instruction);
        Assert.That(craneStack.Stacks.First().ToArray(),Is.Empty);
        CollectionAssert.AreEqual(new[]{"N","Z","P"},craneStack.Stacks.Last().ToArray());
    }
    
    [Test]
    public void Should_process_all_instructions()
    {
        var stackStringWithInstructions = SpecExampleStackString + "\r\n" + SpecExampleInstructions;
        var craneStack = new CraneStack(PuzzleInput.InputStringToArray(stackStringWithInstructions));
        craneStack.ProcessAllInstructions();
        CollectionAssert.AreEqual(new[]{"C"},craneStack.Stacks.First().ToArray());
        CollectionAssert.AreEqual(new[]{"M"},craneStack.Stacks.Skip(1).First().ToArray());
        CollectionAssert.AreEqual(new[]{"Z","N","D","P"},craneStack.Stacks.Last().ToArray());
    }

    [Test]
    public void Should_collate_top_crate_labels_into_string()
    {
        var stackStringWithInstructions = SpecExampleStackString + "\r\n" + SpecExampleInstructions;
        var craneStack = new CraneStack(PuzzleInput.InputStringToArray(stackStringWithInstructions));
        craneStack.ProcessAllInstructions();
        var result = craneStack.GetTopCrates();
        Assert.That(result, Is.EqualTo("CMZ"));
    }

    private const string SpecExampleInstructions = @"move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";

    private static IEnumerable<object[]> StackConfigurations()
    {
        yield return new object[] { StackOneRowOneColumn, 1, new[] { "A" } };
        yield return new object[] { StackTwoRowOneColumn, 2, new[] { "A", "B" } };
        yield return new object[] { StackThreeRowOneColumn, 3, new[] { "C", "B", "A" } };
    }
}