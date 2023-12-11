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

public static class WildCardPoker
{

    public static Tuple<string,string> SelectWildCard(Tuple<string,string> hand)
    {
        var cardsArray = hand.Item1.ToCharArray();
        if (cardsArray.All(c => c != 'J')) return hand;
        //Jokers are Jacks for the purpose of a tie break 
        if (cardsArray.All(c => c == 'J')) return hand;
        
        var strippedOfWildCards = hand.Item1.Replace("J", "");
        var handStrengthWithWildCards = PokerHandComparison.HandStrength(hand.Item1);
        var numberOfJokers = cardsArray.Count(c => c=='J');
        var highestCardSymbol = strippedOfWildCards.MaxBy(c => PokerHandComparison.GetCardRank(c.ToString())).ToString();
        
        switch (handStrengthWithWildCards)
        {
            case HandStrength.Pair:
            {
                //to three of a kind
                var maxBy = strippedOfWildCards.MaxBy(c => cardsArray.Count(card => card==c));
                highestCardSymbol = maxBy.ToString();
                break;
            }
            case HandStrength.TwoPair when numberOfJokers == 2:
            {
                //to 4 of a kind
                var maxBy = strippedOfWildCards.MaxBy(c => cardsArray.Count(card => card==c));
                highestCardSymbol = maxBy.ToString();
                break;
            }
            case HandStrength.ThreeOfAKind when numberOfJokers == 1:
            {
                //to 4 of a kind
                var maxBy = strippedOfWildCards.MaxBy(c => cardsArray.Count(card => card==c));
                highestCardSymbol = maxBy.ToString();
                break;
            }
        }

        return new(hand.Item1.Replace("J", highestCardSymbol), hand.Item2);
    }
}

public class PokerHandComparison : IComparer<Tuple<string, string>>
{
    private readonly IEnumerable<Tuple<string, string>> _originalHandsArray;

    public PokerHandComparison(IEnumerable<Tuple<string, string>> originalHandsArray)
    {
        _originalHandsArray = originalHandsArray;
    }
    public static readonly Dictionary<string, int> SuitedRanks = new()
    {
        {"T" , 10},
        {"J" , 11},
        {"Q" , 12},
        {"K" , 13},
        {"A" , 14}
    };

    private const int Hand2Wins = 1;
    private const int Hand1Wins = -1;
    
    public virtual int Compare(Tuple<string, string>? hand1, Tuple<string, string>? hand2)
    {
        if (hand1 is null) return Hand2Wins;
        if (hand2 is null) return Hand1Wins;

        var hand1Strength = HandStrength(hand1);
        var hand2Strength = HandStrength(hand2);
        if (hand1Strength > hand2Strength) return Hand1Wins;
        if (hand2Strength > hand1Strength) return Hand2Wins;
        
        //Matching rank - highest first distinct card wins
        //But we need to compare the original hand value - not the wildcard
        var hand1Array = _originalHandsArray.Single(hand => hand.Item2 == hand1.Item2).Item1.ToCharArray();
        var hand2Array = _originalHandsArray.Single(hand => hand.Item2 == hand2.Item2).Item1.ToCharArray();
        
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

    public static int GetCardRank(string card)
    {
        if (int.TryParse(card, out var numericRank))
            return numericRank;
        if(!SuitedRanks.ContainsKey(card)) 
            throw new ($"Card {card} is not a face card or numeric");
        
        return SuitedRanks[card];
    }

    public static HandStrength HandStrength(Tuple<string, string> hand)
    {
        return HandStrength(hand.Item1);
    }

    public static HandStrength HandStrength(string hand)
    {
        var charArray = hand.ToCharArray();
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