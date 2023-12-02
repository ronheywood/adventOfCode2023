namespace TestProject1.Helpers;

public static class CubeGame
{
    public static bool CheckCubes(Tuple<string, string> game)
    {
        foreach (var reveal in ThreeReveals(game.Item2))
        {
            if (ColoredCubes(reveal,"red") > 12) return false;
            if (ColoredCubes(reveal,"green") > 13) return false;
            if (ColoredCubes(reveal,"blue") > 14) return false;
        }

        return true;
    }

    private static int ColoredCubes(string reveal, string color)
    {
        var colorValue = reveal.Split(',').Where(c => c.Contains(color)).ToArray();
        if (!colorValue.Any()) return 0;
        
        var cubes = colorValue.Single().Replace(color, "").Trim();
        return int.TryParse(cubes,out var result) ? result : 0;
    }

    private static IEnumerable<string> ThreeReveals(string revealedCubes)
    {
        return revealedCubes.Split(';');
    }

    public static IEnumerable<Tuple<string,string>> WinningGames(IEnumerable<Tuple<string, string>> puzzleTuple)
    {
        return puzzleTuple.Where(CheckCubes);
    }

    public static int SumGameId(IEnumerable<Tuple<string,string>> puzzleTuple)
    {
        return puzzleTuple.Sum(t => int.Parse(t.Item1.Replace("Game ", "")));
    }

    public static int MostCubes(Tuple<string, string> game, string color)
    {
        return ThreeReveals(game.Item2).Select(reveal => ColoredCubes(reveal, color)).Prepend(0).Max();
    }

    public static int SumPower(Tuple<string, string> game)
    {
        return MostCubes(game, "red") * MostCubes(game, "green") * MostCubes(game, "blue");
    }
}