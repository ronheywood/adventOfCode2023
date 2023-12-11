namespace TestProject1.Helpers;

public record MapLocation
{
    public MapLocation(string location)
    {
        var parts = location.Split(" = ");
        Key = parts.First();
        var exits = parts.Last().Split(", ").Select(exit => exit.Replace("(","").Replace(")","")).ToArray();
        Exits = new Tuple<string, string>(exits.First(), exits.Last());
    }
    public string Key { get; }
    private Tuple<string,string> Exits { get; }
    public string Right => Exits.Item2;
    public string Left => Exits.Item1;
}