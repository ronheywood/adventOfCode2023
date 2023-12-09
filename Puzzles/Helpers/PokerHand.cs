namespace TestProject1.Helpers;

public record PokerHand
{
    public PokerHand(Tuple<string, string> hand, int index, int numberOfHands)
    {
        Hand = hand.Item1;
        Bid = hand.Item2;
        Rank = numberOfHands - index;
        BidMultiple = int.Parse(hand.Item2) * Rank;
    }
    
    public string Hand { get; init; }
    public string Bid { get; init; }
    public int Rank { get; init; }
    public int BidMultiple { get; init; }
}