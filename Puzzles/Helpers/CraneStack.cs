using System.Collections;

namespace TestProject1.Helpers;

public class CraneStack
{
    private readonly List<Stack> _stacks;

    public CraneStack(IEnumerable<string> startingConfiguration)
    {
        var configuration = startingConfiguration as string[] ?? startingConfiguration.ToArray();
        _stacks = SetupStacksForCrates(configuration);
        SetupCratesInStacks(configuration);
        Instructions = SetupInstructions(configuration);
    }

    private static IEnumerable<CraneMoveInstruction> SetupInstructions(IEnumerable<string> configuration) =>
        configuration.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1)
            .Select(CraneMoveInstruction.MakeInstructionFromHumanReadableString);

    private void SetupCratesInStacks(IEnumerable<string> configuration)
    {
        var crateRows = CrateRows(configuration).ToArray();

        for (var i = 0; i < _stacks.Count; i++)
        {
            var crateStartIndex = i * 4;
            foreach (var row in crateRows)
            {
                var crate = row.ToCharArray().Skip(crateStartIndex).Take(3).ToArray();
                if (crate.Contains('[')) _stacks[i].Push(new string(crate).Replace("[", "").Replace("]", ""));
            }
        }
    }

    private static IEnumerable<string> CrateRows(IEnumerable<string> configuration) =>
        configuration.TakeWhile(line => line.Contains('[')).Reverse().ToList();

    private static string NumberRow(IEnumerable<string> configuration) =>
        configuration.SkipWhile(line => line.Contains('[')).First();

    private static List<Stack> SetupStacksForCrates(IEnumerable<string> startingConfiguration)
    {
        var numberOfStacks = int.TryParse(CrateStackNumbers(startingConfiguration).Split(" ").Last(), out var num)
            ? num
            : 0;
        var stacks = new List<Stack>();
        for (var i = 0; i < numberOfStacks; i++)
        {
            stacks.Add(new Stack());
        }

        return stacks;
    }

    private static string CrateStackNumbers(IEnumerable<string> startingConfiguration) =>
        NumberRow(startingConfiguration).TrimEnd();

    public IEnumerable<Stack> Stacks => _stacks;

    public IEnumerable<CraneMoveInstruction> Instructions { get; }

    public bool CanMoveMany
    {
        get;
        set;
    }

    public void ProcessInstruction(CraneMoveInstruction instruction)
    {
        var from = _stacks[instruction.TakeFromStack-1];
        var to = _stacks[instruction.PushToStack-1];
        if (CanMoveMany)
        {
            MoveMany(instruction.MoveHowMany, to, from);
            return;
        }
        
        MoveOneAtATime(instruction.MoveHowMany, to, from);
    }

    private static void MoveMany(int howMany, Stack to, Stack from)
    {
        var stack = new List<object>();
        for (var i = howMany; i > 0; i--)
        {
            stack.Add(from.Pop() ?? string.Empty);
        }

        stack.Reverse();
        foreach(var item in stack)
        {
            to.Push(item);
        }
    }

    private static void MoveOneAtATime(int howMany, Stack to, Stack from)
    {
        for (var i = howMany; i > 0; i--)
        {
            to.Push(from.Pop());
        }
    }

    public void ProcessAllInstructions()
    {
        foreach (var craneMoveInstruction in Instructions)
        {
            ProcessInstruction(craneMoveInstruction);
        }
    }

    public string GetTopCrates()
    {
        return _stacks.Aggregate(string.Empty, (current, stack) => current + stack.Pop());
    }
}