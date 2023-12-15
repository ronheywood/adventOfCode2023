namespace TestProject1.Helpers.Tests;

public class SequencePredictionTests
{
    [TestCase("0,3,6,9,12,15","3,3,3,3,3")]
    [TestCase("0,1,2,3","1,1,1")]
    [TestCase("0,2,4,6,8","2,2,2,2")]
    public void Should_extract_differences_for_a_sequence_with_consistent_gaps(string sequence, string expected)
    {
        var input = sequence.Split(',').Select(long.Parse);
        var expectedInts = expected.Split(',').Select(int.Parse);
        CollectionAssert.AreEqual(expectedInts, SequencePrediction.Differences(input));
    }

    [TestCase("1,3,6,10,15,21","2,3,4,5,6")]
    [TestCase("10,13,16,21,30,45","3,3,5,9,15")]
    
    public void Should_extract_differences_for_a_sequence_with_inconsistent_gaps(string sequence, string expected)
    {
        var input = sequence.Split(',').Select(long.Parse);
        var expectedInts = expected.Split(',').Select(int.Parse);
        CollectionAssert.AreEqual(expectedInts, SequencePrediction.Differences(input));
    }

    [Test]
    public void Should_return_sequence_when_no_difference()
    {
        var sequence = "1 1 1 1 1 1";
        var result = SequencePrediction.Recurse(sequence).ToArray();
        Assert.That(result.Count(),Is.EqualTo(1));
        CollectionAssert.AreEqual(new []{1,1,1,1,1,1},result[0]);
    }
    
    [Test]
    public void Should_process_until_sequence_is_all_zero()
    {
        var sequence = "0 3 6 9 12 15";
        var result = SequencePrediction.Recurse(sequence).ToArray();
        Assert.That(result.Count(),Is.EqualTo(2));
        CollectionAssert.AreEqual(new []{0,3,6,9,12,15},result[0]);
        CollectionAssert.AreEqual(new []{3,3,3,3,3},result[1]);
        
        sequence = "1 3 6 10 15 21";
        result = SequencePrediction.Recurse(sequence).ToArray();
        Assert.That(result, Has.Length.EqualTo(3));
        CollectionAssert.AreEqual(new []{1,3,6,10,15,21},result[0]);
        CollectionAssert.AreEqual(new []{2,3,4,5,6},result[1]);
        CollectionAssert.AreEqual(new []{1,1,1,1},result[2]);
    }
    
    [TestCase("1 1 1 1 1 1", 1)]
    [TestCase("5 5 5", 5)]
    [TestCase("10 10 10", 10)]
    public void Should_make_predictions_for_repeated_numbers(string sequence, long expectedPrediction)
    {
        var differences = SequencePrediction.Recurse(sequence);
        var prediction = SequencePrediction.Prediction(differences);
        Assert.That(prediction, Is.EqualTo(expectedPrediction));
    }

    [TestCase("0 3 6 9 12 15", 18)]
    [TestCase("0 1 2 3 4 5", 6)]
    public void Should_make_predictions_for_simple_sequences(string sequence, long expectedPrediction)
    {
        var differences = SequencePrediction.Recurse(sequence);
        var prediction = SequencePrediction.Prediction(differences);
        Assert.That(prediction, Is.EqualTo(expectedPrediction));
    }
    
    [TestCase("5 4 3 2 1", 0)]
    [TestCase("0 -10 -20 -30", -40)]
    public void Should_make_predictions_for_negative_sequences(string sequence, long expectedPrediction)
    {
        var differences = SequencePrediction.Recurse(sequence);
        var prediction = SequencePrediction.Prediction(differences);
        Assert.That(prediction, Is.EqualTo(expectedPrediction));
    }

    [TestCase("0 3 6 9 12 15", 18)]
    [TestCase("0 1 2 3 4 5", 6)]
    [TestCase("1 3 6 10 15 21",28)]
    [TestCase("1 3 6 10 15 21",28)]
    [TestCase("10 13 16 21 30 45",68)]
    
    public void Should_make_predictions_for_larger_sequences(string sequence, long expectedPrediction)
    {
        var differences = SequencePrediction.Recurse(sequence);
        var prediction = SequencePrediction.Prediction(differences);
        Assert.That(prediction, Is.EqualTo(expectedPrediction));
    }

    [Test]
    public void Should_sum_predictions_for_many_sequences()
    {
        var example = @"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45";
        var sequences = PuzzleInput.InputStringToArray(example);
        var sum = sequences.Select(SequencePrediction.Recurse).Select(SequencePrediction.Prediction).Sum();
        Assert.That(sum, Is.EqualTo(114));
    }

    [Test]
    public void Should_sum_predictions_for_puzzle_input()
    {
        var sequences = PuzzleInput.GetFile("day9.txt");
        var sum = sequences.Select(SequencePrediction.Recurse).Select(SequencePrediction.Prediction).Sum();
        Assert.That(sum, Is.EqualTo(1762065988));
    }
}

public class PredictPrecedingShould
{
    [TestCase("1 1 1 1 1 1", 1)]
    [TestCase("5 5 5", 5)]
    [TestCase("10 10 10", 10)]
    public void Should_make_predictions_for_repeated_numbers(string sequence, long expectedPrediction)
    {
        var differences = SequencePrediction.Recurse(sequence);
        var prediction = SequencePrediction.PredictionPrevious(differences);
        Assert.That(prediction, Is.EqualTo(expectedPrediction));
    }
    
    [TestCase("5 4 3 2 1", 6)]
    [TestCase("0 -10 -20 -30", 10)]
    public void Should_make_predictions_for_negative_sequences(string sequence, long expectedPrediction)
    {
        var differences = SequencePrediction.Recurse(sequence);
        var prediction = SequencePrediction.PredictionPrevious(differences);
        Assert.That(prediction, Is.EqualTo(expectedPrediction));
    }

    [TestCase("0 3 6 9 12 15", -3)]
    [TestCase("0 1 2 3 4 5", -1)]
    [TestCase("1 3 6 10 15 21",0)]
    [TestCase("10 13 16 21 30 45",5)]
    
    public void Should_make_predictions_for_larger_sequences(string sequence, long expectedPrediction)
    {
        var differences = SequencePrediction.Recurse(sequence);
        var prediction = SequencePrediction.PredictionPrevious(differences);
        Assert.That(prediction, Is.EqualTo(expectedPrediction));
    }
    
    [Test]
    public void Should_sum_predictions_for_many_sequences()
    {
        var example = @"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45";
        var sequences = PuzzleInput.InputStringToArray(example);
        var sum = sequences.Select(SequencePrediction.Recurse).Select(SequencePrediction.PredictionPrevious).Sum();
        Assert.That(sum, Is.EqualTo(2));
    }

    [Test]
    public void Should_sum_predictions_for_puzzle_input()
    {
        var sequences = PuzzleInput.GetFile("day9.txt");
        var sum = sequences.Select(SequencePrediction.Recurse).Select(SequencePrediction.PredictionPrevious).Sum();
        Assert.That(sum, Is.EqualTo(1066));
    }
}