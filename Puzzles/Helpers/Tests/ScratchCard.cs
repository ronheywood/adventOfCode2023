namespace TestProject1.Helpers.Tests;

public static class ScratchCard
{
    public static IEnumerable<string> Match(Tuple<string, string> gameCard)
    {
        var (game, card) = gameCard;
        var tables = GetGameTables(card).ToArray();
        var matches = tables.First().Intersect(tables.Last());
        return matches;
    }

    public static IEnumerable<string[]> GetGameTables(string card)
    {
        var cards = card.Split(" | ");
        var winning = cards[0].Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        var matches = cards[1].Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        return new[] { winning, matches };
    }

    public static int Score(string[] matches)
    {
        return matches.Any() ? matches.Aggregate(0, (current, match) => (current == 0) ? 1 : current * 2) : 0;
    }
}