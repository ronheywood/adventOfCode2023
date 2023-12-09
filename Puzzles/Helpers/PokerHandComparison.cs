namespace TestProject1.Helpers;


public enum HandStrength
{
    HighCard = 0,
    Pair = 1,
    TwoPair = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6
}
public class PokerHandComparison : IComparer<Tuple<string, string>>
{
    private const int Hand2Wins = 1;
    private const int Hand1Wins = -1;
    
    public int Compare(Tuple<string, string>? hand1, Tuple<string, string>? hand2)
    {
        if (hand1 is null) return Hand2Wins;
        if (hand2 is null) return Hand1Wins;

        var hand1Strength = HandStrength(hand1);
        var hand2Strength = HandStrength(hand2);
        if (hand1Strength > hand2Strength) return Hand1Wins;
        if (hand2Strength > hand1Strength) return Hand2Wins;
        
        //Matching rank - highest first distinct card wins 
        var hand1Array = hand1.Item1.ToCharArray();
        var hand2Array = hand2.Item1.ToCharArray();
        var card1 = 0; var card2 = 0; var index = 0;
        while (card1 == card2 && index < hand1Array.Length)
        {
            card1 = GetCardRank(hand1Array[index].ToString());
            card2 = GetCardRank(hand2Array[index].ToString());
            if (card1 > card2) return Hand1Wins;
            if (card1 < card2) return Hand2Wins;
            index++;
        }

        return 0;
    }

    private static int GetCardRank(string card)
    {
        Dictionary<string,int> suitedRank = new()
        {
            {"T" , 10},
            {"J" , 11},
            {"Q" , 12},
            {"K" , 13},
            {"A" , 14}
        };
        if (int.TryParse(card, out var numericRank))
            return numericRank;
        if(!suitedRank.ContainsKey(card)) 
            throw new ($"Card {card} is not a face card or numeric");
        
        return suitedRank[card];
    }

    public static HandStrength HandStrength(Tuple<string, string> hand)
    {
        var charArray = hand.Item1.ToCharArray();
        var distinct = charArray.Distinct().ToArray();
        return distinct.Length switch
        {
            4 => Helpers.HandStrength.Pair,
            3 =>
                //Three of a kind or two pair
                distinct.Any(card => charArray.Count(c => c == card) == 3)
                    ? Helpers.HandStrength.ThreeOfAKind
                    : Helpers.HandStrength.TwoPair,
            2 =>
                //FullHouse or Four of a kind
                distinct.Any(card => charArray.Count(c => c == card) == 3)
                    ? Helpers.HandStrength.FullHouse
                    : Helpers.HandStrength.FourOfAKind,
            1 => Helpers.HandStrength.FiveOfAKind,
            _ => Helpers.HandStrength.HighCard
        };
    }
}