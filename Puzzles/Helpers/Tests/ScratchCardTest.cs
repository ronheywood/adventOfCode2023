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
}