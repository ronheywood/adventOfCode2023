namespace TestProject1.Helpers;

public class PokerGameShould
{
    private const string ExamplePuzzleInput = @"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483";

    [Test]
    public void Should_get_hands_and_bids()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var handsToBid = PuzzleInput.GetPuzzlePairs(puzzleLines," ").ToArray();
        var hands = handsToBid.Select(p => p.Item1);
        var bids = handsToBid.Select(p => p.Item2);
        CollectionAssert.AreEqual(new[]{"32T3K","T55J5","KK677","KTJJT","QQQJA"},hands);
        CollectionAssert.AreEqual(new[]{"765","684","28","220","483"},bids);
    }

    [Test]
    public void Should_add_rank_1_to_single_hand()
    {
        var hands = new[] { new Tuple<string, string>(@"32T3K", "765") };
        var result = PokerGame.RankHands(hands);
        var expected = new PokerHand(new Tuple<string, string>("32T3K","765"),0,1)
        {
            Hand = "32T3K",
            Bid = "765",
            Rank = 1,
            BidMultiple = 765 
        };
        
        Assert.That(result.First(),Is.EqualTo(expected));
    }
    
    [Test]
    public void Should_add_rank_and_multiple_when_multiple_hands_in_strength_order()
    {
        var hands = new[] { new Tuple<string, string>(@"42T5K", "765"), new Tuple<string, string>(@"32T5K", "765"), };
        var result = PokerGame.RankHands(hands).ToArray();
        var expected1 = new PokerHand(new Tuple<string, string>("42T4K","765"),0, 2)
        {
            Hand = "42T5K",
            Bid = "765",
            Rank = 2,
            BidMultiple = 1530 
        };
        var expected2 = new PokerHand(new Tuple<string, string>("32T3K","765"),1, 2)
        {
            Hand = "32T5K",
            Bid = "765",
            Rank = 1,
            BidMultiple = 765 
        };
        
        Assert.Multiple(() =>
        {
            Assert.That(result.First(), Is.EqualTo(expected1));
            Assert.That(result.Last(), Is.EqualTo(expected2));
        });
    }

    [Test]
    public void Should_order_cards_by_highest_first_card_when_rank_is_high_card()
    {
        var hands = new[] { new Tuple<string, string>(@"32T4K", "765"), new Tuple<string, string>(@"42T3K", "765"), };
        var result = PokerGame.RankHands(hands).ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo("42T3K"));
            Assert.That(result.Last().Hand, Is.EqualTo("32T4K"));
        });
    }
    
    [Test]
    public void Should_order_cards_by_second_card_order_when_rank_is_high_card_and_first_card_macthes()
    {
        var hands = new[] { new Tuple<string, string>(@"32T5K", "765"), new Tuple<string, string>(@"35T4K", "765"), };
        var result = PokerGame.RankHands(hands).ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo("35T4K"));
            Assert.That(result.Last().Hand, Is.EqualTo("32T5K"));
        });
    }
    
    [Test]
    public void Should_order_cards_by_third_card_order_when_rank_is_pair_and_first_pair_matches()
    {
        var hands = new[] { new Tuple<string, string>(@"3325K", "765"), new Tuple<string, string>(@"3342K", "765"), };
        var result = PokerGame.RankHands(hands).ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo("3342K"));
            Assert.That(result.Last().Hand, Is.EqualTo("3325K"));
        });
    }
    
    [Test]
    public void Should_order_cards_by_fourth_card_order_when_rank_is_three_of_a_kind_and_first_set_matches()
    {
        var hands = new[] { new Tuple<string, string>(@"3332K", "765"), new Tuple<string, string>(@"3335K", "765"), };
        var result = PokerGame.RankHands(hands).ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo("3335K"));
            Assert.That(result.Last().Hand, Is.EqualTo("3332K"));
        });
    }
    
    [Test]
    public void Should_order_cards_by_fifth_card_order_when_rank_is_four_of_a_kind_and_first_set_matches()
    {
        var hands = new[] { new Tuple<string, string>(@"33335", "765"), new Tuple<string, string>(@"33332", "765"), };
        var result = PokerGame.RankHands(hands).ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo("33335"));
            Assert.That(result.Last().Hand, Is.EqualTo("33332"));
        });
    }
    
    [Test]
    public void Should_retain_order_for_five_of_a_kind_matching()
    {
        var hands = new[] { new Tuple<string, string>(@"33333", "2"), new Tuple<string, string>(@"33333", "1"), };
        var result = PokerGame.RankHands(hands).ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo("33333"));
            Assert.That(result.First().Bid, Is.EqualTo("2"));
            Assert.That(result.Last().Hand, Is.EqualTo("33333"));
            Assert.That(result.Last().Bid, Is.EqualTo("1"));
        });
    }
    
    [TestCase("J2345")]
    [TestCase("Q2345")]
    [TestCase("K2345")]
    [TestCase("A2345")]
    public void Should_rank_face_cards_over_number_cards(string hand1)
    {
        var hands = new[] { new Tuple<string, string>(hand1, "2"), new Tuple<string, string>(@"23459", "1"), };
        var result = PokerGame.RankHands(hands).ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo(hand1));
            Assert.That(result.Last().Hand, Is.EqualTo("23459"));
        });
    }
    
    [TestCase("J2345","Q2345")]
    [TestCase("Q2345","K2345")]
    [TestCase("K2345","A2346")]
    [TestCase("23457","A2345")]
    public void Should_order_by_face_cards(string hand1, string hand2)
    {
        var hands = new[] { new Tuple<string, string>(hand1, "2"), new Tuple<string, string>(hand2, "1"), };
        var result = PokerGame.RankHands(hands).ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo(hand2));
            Assert.That(result.Last().Hand, Is.EqualTo(hand1));
        });
    }

    [Test]
    public void Should_rank_low_pair_over_high_card()
    {
        var hands = new[] { new Tuple<string, string>("A2543", "2"), new Tuple<string, string>("32245", "1"), };
        var result = PokerGame.RankHands(hands).ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo("32245"));
            Assert.That(result.Last().Hand, Is.EqualTo("A2543"));
        });
    }
}

public static class PokerGame
{
    public static IEnumerable<PokerHand> RankHands(IEnumerable<Tuple<string, string>> hands)
    {
        var handsArray = hands.ToArray(); 
        var numberOfHands = handsArray.Length;
        return handsArray.OrderBy(hand => hand, new PokerHandComparison()).Select((hand,index) => new PokerHand(hand,index,numberOfHands));
    }
}

public class PokerHandComparisonShould
{
    [Test]
    public void Identify_hands_with_a_pair()
    {
        var hand = new Tuple<string, string>("22345","");
        Assert.That(PokerHandComparison.IsPair(hand),Is.True);
    }

    [Test]
    public void Three_of_a_kind()
    {
        var hand = new Tuple<string, string>("22245","");
        Assert.That(PokerHandComparison.IsPair(hand),Is.False);
    }
    
    [Test]
    public void Four_of_a_kind()
    {
        var hand = new Tuple<string, string>("22225","");
        Assert.That(PokerHandComparison.IsPair(hand),Is.False);
    }
    
    [Test]
    public void Five_of_a_kind()
    {
        var hand = new Tuple<string, string>("22225","");
        Assert.That(PokerHandComparison.IsPair(hand),Is.False);
    }
    
    [Test]
    public void Two_pair()
    {
        var hand = new Tuple<string, string>("22335","");
        Assert.That(PokerHandComparison.IsPair(hand),Is.False);
    }
    
    [TestCase("22333")]
    public void Full_house(string fullHouse)
    {
        var hand = new Tuple<string, string>(fullHouse,"");
        Assert.That(PokerHandComparison.IsPair(hand),Is.False);
    }
}

public class PokerHandComparison : IComparer<Tuple<string, string>>
{
    private const int Hand2Wins = 1;
    private const int Hand1Wins = -1;
    
    public int Compare(Tuple<string, string>? hand1, Tuple<string, string>? hand2)
    {
        if (hand1 is null) return Hand2Wins;
        if (hand2 is null) return Hand1Wins;

        if (IsPair(hand2)) return Hand2Wins;
        //if (IsPair(hand1)) return Hand1Wins;
        
        //High Card
        var hand1Array = hand1.Item1.ToCharArray();
        var hand2Array = hand2.Item1.ToCharArray();
        
        var card1 = 0; var card2 = 0; var index = 0;
        while (card1 == card2 && index < hand1Array.Length)
        {
            card1 = GetRank(hand1Array[index].ToString());
            card2 = GetRank(hand2Array[index].ToString());
            if (card1 > card2) return Hand1Wins;
            if (card1 < card2) return Hand2Wins;
            index++;
        }

        return 0;
    }

    public static bool IsPair(Tuple<string, string> hand1)
    {
        var cards = hand1.Item1.ToCharArray();
        var distinctCards = cards.Distinct();
        return distinctCards.Count() ==4;
    }

    private static int GetRank(string card)
    {
        Dictionary<string,int> suitedRank = new()
        {
            {"J" , 10},
            {"Q" , 11},
            {"K" , 12},
            {"A" , 13}
        };
        return int.TryParse(card,out var numericRank) ? numericRank : suitedRank[card];
    }
}

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