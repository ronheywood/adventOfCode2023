using NUnit.Framework.Constraints;

namespace TestProject1.Helpers.Tests;

public class ScratchCardTest
{
    private const string ExamplePuzzleInput = @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

    [Test]
    public async Task Should_identify_game_and_card()
    {
        var puzzleInput = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":");
        await Verify(pairs);
    }

    [Test]
    public async Task Should_extract_game_tables_from_card_removing_any_leading_spaces_from_single_digits()
    {
        var simpleGame = @"Card 1:  1 48 83 86  2 | 83 86  6 31 17  9 48 53";
        var puzzleInput = PuzzleInput.InputStringToArray(simpleGame);
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        var card = ScratchCard.GetGameTables(pairs[0].Item2);
        await Verify(card);
    }

    [TestCase("Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36")]
    [TestCase("Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11")]
    public void Should_identify_when_no_numbers_from_the_right_match_numbers_on_the_left(string noMatch)
    {
        var puzzleInput = PuzzleInput.InputStringToArray(noMatch);
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        Assert.That(ScratchCard.Match(pairs[0]), Is.Empty);
    }
    
    [TestCase("Card 1: 41 42 43 44 45 | 41 11 12 13 14 15 16 17",new[]{"41"})]
    [TestCase("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",new[]{"48", "83", "86","17"})]
    [TestCase("Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",new[]{"32", "61"})]
    [TestCase("Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",new[]{"1", "21"})]
    [TestCase("Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",new[]{"84"})]
    
    public void Should_identify_when_numbers_from_the_right_match_numbers_on_the_left(string card,string[] expected)
    {
        var puzzleInput = PuzzleInput.InputStringToArray(card);
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        CollectionAssert.AreEqual(expected,ScratchCard.Match(pairs[0]));
    }

    [Test]
    public void Should_score_zero_for_no_match()
    {
        var noMatch = Array.Empty<string>();
        Assert.That(ScratchCard.Score(noMatch),Is.EqualTo(0));
    }
    
    [Test]
    public void Should_score_one_for_one_match()
    {
        var oneMatch = new[] { "41" };
        Assert.That(ScratchCard.Score(oneMatch),Is.EqualTo(1));
    }
    
    [Test]
    public void Should_score_one_doubled_for_two_match()
    {
        var twoMatch = new[] { "41","42" };
        Assert.That(ScratchCard.Score(twoMatch),Is.EqualTo(2));
    }
    
    [Test]
    public void Should_score_one_doubled_twice_for_three_matches()
    {
        var twoMatch = new[] { "41","42","43" };
        Assert.That(ScratchCard.Score(twoMatch),Is.EqualTo(4));
    }
    
    [Test]
    public void Should_score_one_doubled_three_times_for_four_matches()
    {
        var twoMatch = new[] { "41","42","43", "45" };
        Assert.That(ScratchCard.Score(twoMatch),Is.EqualTo(8));
    }
    
    [Test]
    public void Should_score_one_doubled_four_times_for_five_matches()
    {
        var twoMatch = new[] { "41","42","43", "45", "46" };
        Assert.That(ScratchCard.Score(twoMatch),Is.EqualTo(16));
    }

    [Test]
    public void Should_sum_all_the_scores_for_the_example()
    {
        var puzzleInput = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        var total = 0;
        foreach (var cardTable in pairs)
        {
            var matches = ScratchCard.Match(cardTable).ToArray();
            total += ScratchCard.Score(matches);
        }
        Assert.That(total,Is.EqualTo(13));
    }
    
    [Test]
    public void Should_sum_all_the_scores_for_the_puzzle_input()
    {
        var puzzleInput = PuzzleInput.GetFile("day4.txt");
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        var total = 0;
        foreach (var cardTable in pairs)
        {
            var matches = ScratchCard.Match(cardTable).ToArray();
            total += ScratchCard.Score(matches);
        }
        Assert.That(total,Is.EqualTo(27454));
    }

    [Test]
    public void Should_make_no_copies_of_cards_if_no_matches()
    {
        var puzzleInput = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        var cardWithNoMatches = pairs.Last();
        Assert.That(ScratchCard.GetCopiedCards(pairs,cardWithNoMatches),Is.Empty);
    }
    
    [Test]
    public void Should_make_one_copy_of_cards_if_one_matches()
    {
        var puzzleInput = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        var cardWithOneMatch = pairs[3];
        Assert.That(ScratchCard.GetCopiedCards(pairs,cardWithOneMatch),Is.Not.Empty);
        Assert.That(ScratchCard.GetCopiedCards(pairs,cardWithOneMatch).Single(),Is.EqualTo(pairs[4]));
    }

    [Test]
    public void Should_make_two_copies_of_cards_if_two_matches()
    {
        var puzzleInput = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        var cardWithTwoMatches = pairs[1];
        var copiedCards = ScratchCard.GetCopiedCards(pairs,cardWithTwoMatches);
        //Assert.That(copiedCards.Count(),Is.EqualTo(2));
        CollectionAssert.AreEqual(new[]{pairs[2],pairs[3]},copiedCards);
    }

    [Test]
    public void Should_make_four_copies_of_cards_if_four_matches()
    {
        var puzzleInput = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        var cardWithFourMatches = pairs[0];
        var copiedCards = ScratchCard.GetCopiedCards(pairs,cardWithFourMatches);
        CollectionAssert.AreEqual(new[]{pairs[1],pairs[2],pairs[3],pairs[4]},copiedCards);
    }

    [Test]
    public void Should_make_no_extra_cards_when_all_cards_lose()
    {
        string input = @"Card 1: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 2: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 3: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 4: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 5: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 6: 41 48 83 86 17 |  1  2  3  4  5  6  7  8";
        var puzzleInput = PuzzleInput.InputStringToArray(input);
        var originalCards = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        var copiedCards = ScratchCard.PlayAllCards(originalCards);
        CollectionAssert.AreEqual(originalCards,copiedCards);
    }
    
    [Test]
    [Ignore("Cards will never make you copy a card past the end of the table")]
    public void Should_make_no_extra_cards_when_only_last_card_wins()
    {
        var input = @"Card 1: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 2: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 3: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 4: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 5: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 6: 41 48 83 86 17 |  41  2  3  4  5  6  7  8";
        var puzzleInput = PuzzleInput.InputStringToArray(input);
        var originalCards = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        var copiedCards = ScratchCard.PlayAllCards(originalCards);
        CollectionAssert.AreEqual(originalCards,copiedCards);
    }

    [Test]
    public async Task Should_add_a_copy_of_the_last_card_when_the_penultimate_card_wins()
    {
        var input = @"Card 1: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 2: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 3: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 4: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 5: 41 48 83 86 17 |  41  2  3  4  5  6  7  8
Card 6: 41 48 83 86 17 |  1  2  3  4  5  6  7  8";
        var puzzleInput = PuzzleInput.InputStringToArray(input);
        var originalCards = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToList();
        var expected = originalCards.ToArray().ToList();
        expected.Add(originalCards.Last() );
        
        var copiedCards = ScratchCard.PlayAllCards(originalCards);
        await Verify(copiedCards);
        //CollectionAssert.AreEqual(expected,copiedCards);
    }

    [Test]
    public async Task Should_make_recursive_copies()
    {
        var winRecursive = @"Card 1: 41 48 83 86 17 | 41 48  3  4  5  6  7  8
Card 2: 41 48 83 86 17 | 41  2  3  4  5  6  7  8
Card 3: 41 48 83 86 17 | 41  2  3  4  5  6  7  8
Card 4: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 5: 41 48 83 86 17 |  1  2  3  4  5  6  7  8
Card 6: 41 48 83 86 17 |  1  2  3  4  5  6  7  8";
        //card one wins a copy of 2 and three, copy of 2 wins a copy of 3, copy of 3 wins a copy of 4 => adds Card 2, Card 3, Card 3, Card 4
        //card two wins a copy of three, copy of 3 wins a copy of 4 => Adds Card 3, Card 4
        //card three wins a copy of 4, copy of 4 wins nothing => Adds Card 4
        //4 five six win nothing
        var puzzleInput = PuzzleInput.InputStringToArray(winRecursive);
        var originalCards = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToList();
        var expected = originalCards.ToArray().ToList();
        expected.Add(originalCards.Last() );
        
        var copiedCards = ScratchCard.PlayAllCards(originalCards);
        Assert.That(copiedCards.Count(c => c.Item1 == "Card 1"),Is.EqualTo(1));
        
        Assert.That(copiedCards.Count(c => c.Item1 == "Card 2"),Is.EqualTo(2), "Card 1 wins a copy of card 2 and three");
        
        Assert.That(copiedCards.Count(c => c.Item1 == "Card 3"),Is.EqualTo(4), "Recursive");
        //Assert.That(copiedCards.Count(c => c.Item1 == "Card 4"),Is.EqualTo(3), "Recursive");
        
        await Verify(copiedCards);
    }

    [Test]
    public async Task Should_win_30_cards_from_input()
    {
        var puzzleInput = PuzzleInput.InputStringToArray(ExamplePuzzleInput);
        var originalCards = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToList();
        var expected = originalCards.ToArray().ToList();
        expected.Add(originalCards.Last() );
        
        var copiedCards = ScratchCard.PlayAllCards(originalCards);
        Assert.Multiple(() =>
        {
            Assert.That(copiedCards.Count(), Is.EqualTo(30));
            Assert.That(copiedCards.Count(c => c.Item1 == "Card 1"), Is.EqualTo(1));
            Assert.That(copiedCards.Count(c => c.Item1 == "Card 2"), Is.EqualTo(2));
            Assert.That(copiedCards.Count(c => c.Item1 == "Card 3"), Is.EqualTo(4));
            Assert.That(copiedCards.Count(c => c.Item1 == "Card 4"), Is.EqualTo(8));
            Assert.That(copiedCards.Count(c => c.Item1 == "Card 5"), Is.EqualTo(14));
            Assert.That(copiedCards.Count(c => c.Item1 == "Card 6"), Is.EqualTo(1));
        });
        await Verify(copiedCards);
    }

    [Test]
    [Ignore("After lots of recursion (1m 13 seconds) - 5701343 is too low!")]
    public void Should_win_how_many_cards_from_puzzle_input()
    {   
        var puzzleInput = PuzzleInput.GetFile("day4.txt");
        var originalCards = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToList();
        var expected = originalCards.ToArray().ToList();
        expected.Add(originalCards.Last() );
        
        var copiedCards = ScratchCard.PlayAllCards(originalCards);
        
        Assert.That(copiedCards.Count(),Is.GreaterThan(5701343));
        Assert.That(copiedCards.Count(),Is.EqualTo(6857330));
        
    }
}