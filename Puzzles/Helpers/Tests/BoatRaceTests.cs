namespace TestProject1.Helpers.Tests;

public class BoatRaceTests
{
    [Test]
    public void boats_charged_for_zero_travels_zero()
    {
        var boat = new Boat();
        Assert.That(boat.Speed, Is.EqualTo(0));
        Assert.That(boat.Distance, Is.EqualTo(0));
    }

    [Test]
    public void boats_charged_for_1_travel_at_speed_1()
    {
        var boat = new Boat() { Time = 7 };
        boat.Charge(1);
        Assert.That(boat.Speed, Is.EqualTo(1));
    }

    [TestCase(1,6)]
    [TestCase(2,10)]
    [TestCase(3,12)]
    [TestCase(4,12)]
    [TestCase(5,10)]
    [TestCase(6,6)]
    [TestCase(7,0)]
    public void boats_distance_is_speed_over_time_remaining_for_the_race(int chargeTime, int expectedDistance)
    {
        var boat = new Boat(){ Time = 7 };
        boat.Charge(chargeTime);
        Assert.That(boat.Distance, Is.EqualTo(expectedDistance));
    }

    [Test]
    public void A_boat_charged_for_longer_than_the_time_travels_no_distance()
    {
        var boat = new Boat(){ Time = 7 };
        boat.Time = 7;
        boat.Charge(7);
        Assert.That(boat.Distance,Is.EqualTo(0));
        
        boat.Charge(8);
        Assert.That(boat.Distance,Is.EqualTo(0));
    }

    [Test]
    public void boat_reports_charge_times_and_distances()
    {
        var boat = new Boat(){ Time = 7 };
        var chargeTimes = boat.ChargeTimes().ToArray();
        CollectionAssert.AreEqual(new[]{ 1,2,3,4,5,6}, chargeTimes.Select(t => t.Item1));
        CollectionAssert.AreEqual(new[]{ 6,10,12,12,10,6}, chargeTimes.Select(t => t.Item2));
    }

    [TestCase(7,9,4)]
    public void margin_of_error_from_number_of_ways_to_beat_the_record(int time,int record,int waysToWin)
    {
        var boat = new Boat(){ Time = time };
        var chargeTimes = boat.ChargeTimes().ToArray();
        var wins = chargeTimes.Where(t => t.Item2 > record);
        Assert.That(wins.Count(),Is.EqualTo(waysToWin));
    }
    
    [TestCase(71530,940200,71503)]
    //[TestCase(55999793,401148522741405,71503)]//71503 is too low
    public void Ways_to_win_for_part_2(int time,long record,int waysToWin)
    {
        var boat = new Boat(){ Time = time };
        var chargeTimes = boat.ChargeTimes().ToArray(); //55999792
        
        var wins = chargeTimes.Where(t => t.Item2 > record);
        Assert.That(wins.Count(),Is.EqualTo(waysToWin));
    }

    [TestCase(ExamplePuzzleInput,288)]
    [TestCase(Day6PuzzleInput,2374848)]
    public void margin_of_error_is_multiple_of_number_of_ways_to_win(string puzzleInputString, int expected)
    {
        var puzzleInput = PuzzleInput.InputStringToArray(puzzleInputString).ToArray();
        var pairs = PuzzleInput.GetPuzzlePairs(puzzleInput,":").ToArray();
        var times = pairs[0].Item2.Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();
        var records = pairs[1].Item2.Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();
        // CollectionAssert.AreEqual(new []{"7","15","30"},times);
        // CollectionAssert.AreEqual(new []{"9","40","200"},records);

        var wins = 1;
        for (var i = 0; i < times.Length; i++)
        {
            var boat = new Boat() { Time = int.Parse(times[i]) };
            var record = int.Parse(records[i]);
            var waysToWin = boat.ChargeTimes().Count(t => t.Item2 > record);
            wins *= waysToWin;
        }
        Assert.That(wins,Is.EqualTo(expected));
    }
    
    const string ExamplePuzzleInput = @"Time:      7  15   30
Distance:  9  40  200";

    private const string Day6PuzzleInput = @"Time:        55     99     97     93
Distance:   401   1485   2274   1405";
}