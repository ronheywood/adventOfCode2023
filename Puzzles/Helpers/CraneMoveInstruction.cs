namespace TestProject1.Helpers;

public class CraneMoveInstruction
{
    public int MoveHowMany { get; set; }
    public int TakeFromStack { get; set; }
    public int PushToStack { get; set; }

    public static CraneMoveInstruction MakeInstructionFromHumanReadableString(string line)
    {
        var spaceSeparated = line.Replace("move ", "").Replace("from ", "").Replace("to ", "");
        var instructions = spaceSeparated.Split(" ");
        return new CraneMoveInstruction
        {
            MoveHowMany = int.Parse(instructions[0]),
            TakeFromStack = int.Parse(instructions[1]),
            PushToStack = int.Parse(instructions[2])
        };
    }
}