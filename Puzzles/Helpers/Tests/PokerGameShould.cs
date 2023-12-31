﻿namespace TestProject1.Helpers.Tests;

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
    public void Should_order_cards_by_second_card_order_when_rank_is_high_card_and_first_card_matches()
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
    
    [TestCase("KKKKK","AAAAA",2)]
    [TestCase("QQQQQ","22222",1)]
    [TestCase("22222","QQQQQ",2)]
    public void Should_order_five_of_a_kind_by_high_card(string hand1, string hand2,int winningHandIndex)
    {
        var hands = new[] { new Tuple<string, string>(hand1, "2"), new Tuple<string, string>(hand2, "1"), };
        var result = PokerGame.RankHands(hands).ToArray();
        var winningHand = hands[winningHandIndex - 1];
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo(winningHand.Item1));
        });
    }
    
    [TestCase("KKKKK","23459",1)]
    [TestCase("KKKKK","A3459",1)]
    // [TestCase("QQQQQ","22222",1)]
    // [TestCase("22222","QQQQQ",2)]
    public void Should_order_five_of_a_kind_over_lesser_hands(string hand1, string hand2,int winningHandIndex)
    {
        var hands = new[] { new Tuple<string, string>(hand1, "2"), new Tuple<string, string>(hand2, "1"), };
        var result = PokerGame.RankHands(hands).ToArray();
        var winningHand = hands[winningHandIndex - 1];
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Hand, Is.EqualTo(winningHand.Item1));
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
            Assert.That(result.First().Rank, Is.EqualTo(2));
            Assert.That(result.Last().Hand, Is.EqualTo("A2543"));
        });
    }

    [Test]
    public void Should_rank_example_hand()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var pokerGame = PuzzleInput.GetPuzzlePairs(puzzleLines," ").ToArray();
        var pokerHands = PokerGame.RankHands(pokerGame).ToArray();
        CollectionAssert.AreEqual(new [] {5,4,3,2,1},pokerHands.Select(hand => hand.Rank));
        Assert.That(pokerHands.Select(hand => hand.BidMultiple).Sum(),Is.EqualTo(6440));
    }

    [Test]
    public void Should_rank_puzzle_hand()
    {
        var puzzleLines = PuzzleInput.GetFile("day7.txt");
        var pokerGame = PuzzleInput.GetPuzzlePairs(puzzleLines," ").ToArray();
        var pokerHands = PokerGame.RankHands(pokerGame).ToArray();
        Assert.That(pokerHands.Select(hand => hand.BidMultiple).Sum(),Is.EqualTo(250602641));
    }
    
    [Test]
    public void Should_rank_example_hand_with_wildcards()
    {
        var puzzleLines = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var pokerGame = PuzzleInput.GetPuzzlePairs(puzzleLines," ").ToArray();
        var pokerHands = PokerGame.RankHandsWithWildCards(pokerGame).ToArray();
        Assert.That(pokerHands.Select(hand => hand.BidMultiple).Sum(),Is.EqualTo(5905));
    }
    
    [Test]
    [Ignore("250964151 is too low ...")]
    public void Should_rank_puzzle_hand_with_wildcards()
    {
        var puzzleLines = PuzzleInput.GetFile("day7.txt");
        var pokerGame = PuzzleInput.GetPuzzlePairs(puzzleLines," ").ToArray();
        var pokerHands = PokerGame.RankHandsWithWildCards(pokerGame).ToArray();
        Assert.That(pokerHands.Select(hand => hand.BidMultiple).Sum(),Is.EqualTo(5905));
    }

    [Test]
    public void The_bids_are_unique_and_we_can_use_them_to_find_the_wildcard_hand_and_settle_a_tie()
    {
        var puzzleLines = PuzzleInput.GetFile("day7.txt");
        var pokerGame = PuzzleInput.GetPuzzlePairs(puzzleLines," ").ToArray();
        var allBids = pokerGame.Select(game => game.Item2).ToArray();
        var distinctBids = pokerGame.Select(game => game.Item2).Distinct().ToArray();
        Assert.That(allBids.Length, Is.EqualTo(distinctBids.Length));
    }

    [Test]
    public void When_hands_match_ranks_should_rank_by_strongest_original_first_card()
    {
        var hand1 = "JKKK2";
        var hand2 = "QQQQ2";
        
        var hands = new[] { new Tuple<string, string>(hand1, "2"), new Tuple<string, string>(hand2, "1"), };
        var result = PokerGame.RankHandsWithWildCards(hands).ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(result.First().Rank, Is.EqualTo(2));
            Assert.That(result.First().Hand, Is.EqualTo(hand2));
            Assert.That(result.Last().Hand, Is.EqualTo("KKKK2"));
            Assert.That(result.Last().Rank, Is.EqualTo(1));
        });
    }
}