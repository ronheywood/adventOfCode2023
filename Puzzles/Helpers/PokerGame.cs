﻿namespace TestProject1.Helpers;

public static class PokerGame
{
    public static IEnumerable<PokerHand> RankHands(IEnumerable<Tuple<string, string>> hands)
    {
        var handsArray = hands.ToArray(); 
        var numberOfHands = handsArray.Length;
        return handsArray.OrderBy(hand => hand, new PokerHandComparison(handsArray)).Select((hand,index) => new PokerHand(hand,index,numberOfHands));
    }

    public static IEnumerable<PokerHand> RankHandsWithWildCards(Tuple<string, string>[] hands)
    {
        var handsArray = hands.ToArray(); 
        var numberOfHands = handsArray.Length;
        return handsArray.Select(WildCardPoker.SelectWildCard)
            .OrderBy(hand => hand, new PokerHandComparison(handsArray))
            .Select((hand,index) => new PokerHand(hand,index,numberOfHands));
    }
}