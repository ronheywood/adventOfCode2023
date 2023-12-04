using System.Collections;
using NUnit.Framework.Constraints;

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

    public static IEnumerable<Tuple<string,string>> GetCopiedCards(IEnumerable<Tuple<string,string>> allCards,Tuple<string, string> winningCard)
    {
        var matches = Match(winningCard).ToArray();
        allCards = allCards.ToArray();
        if (!matches.Any()) return Enumerable.Empty<Tuple<string, string>>();
        
        var copiedCards = new List<Tuple<string, string>>();
        var offset = 1;
        foreach (var match in matches)
        {
            var cardNumber = int.Parse(winningCard.Item1.Split(" ").Last()) + offset;
            var nextCard = "Card" + cardNumber;
            //Cards will never make you copy a card past the end of the table?
            //if(allCards.All(card => card.Item1 != nextCard)) break;
            try
            {
                copiedCards.Add(allCards.First(card => card.Item1.Replace(" ","") == nextCard));
            }
            catch (InvalidOperationException)
            {
                throw new Exception($"Attempting to retrieve {nextCard} but it is not in the deck");
            }

             
            offset++;
        }

        return copiedCards;
    }

    public static IEnumerable<Tuple<string,string>> PlayAllCards(IEnumerable<Tuple<string, string>> cardDeck)
    {
        var allCards = cardDeck.ToList();
        var length = allCards.Count;
        if (length == 0) return allCards;
        for(var i = 0;i<allCards.Count;i++)
        {
            if (i > 10000000) throw new Exception($"Over 10 million iterations - possible infinite recursion {length}");
            var card = allCards[i];
            allCards.AddRange( GetCopiedCards(cardDeck,card) );
            length = allCards.Count;
        }
        
        return allCards;
    }
}