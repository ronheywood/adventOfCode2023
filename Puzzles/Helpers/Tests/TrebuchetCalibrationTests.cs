namespace TestProject1.Helpers.Tests;

public class TrebuchetCalibrationTests
{
    public class TrebuchetCalibrationShould
    {
        [TestCase("two1nine","2")]
        [TestCase("eightwothree","8")]
        [TestCase("abcone2threexyz","1")]
        [TestCase("xtwone3four","2")]
        [TestCase("4nineeightseven2","4")]
        [TestCase("zoneight234","1")]
        [TestCase("7pqrstsixteen","7")]
        [TestCase("nowordsorumbers","")]
        public void Should_find_the_first_number_or_word(string example, string expected)
        {
            var actual = TrebuchetCalibration.FirstWordOrNumber(example);
            Assert.That(actual,Is.EqualTo(expected));
        }
        
        [TestCase("two1nine","9")]
        [TestCase("eightwothree","3")]
        [TestCase("abcone2threexyz","3")]
        [TestCase("xtwone3four","4")]
        [TestCase("4nineeightseven2","2")]
        [TestCase("zoneight234","4")]
        [TestCase("7pqrstsixteen","6")]
        [TestCase("nowordsorumbers","")]
        public void Should_find_the_last_number_or_word(string example, string expected)
        {
            var actual = TrebuchetCalibration.LastWordOrNumber(example);
            Assert.That(actual,Is.EqualTo(expected));
        }

        [TestCase("1abc2","12")]
        [TestCase("pqr3stu8vwx","38")]
        [TestCase("a1b2c3d4e5f","15")]
        [TestCase("treb7uchet","77")]
        public void Combine_first_and_last_number(string input,string expected)
        {
            var calibration = TrebuchetCalibration.Calibrate(input);
            Assert.That(calibration, Is.EqualTo(expected));
        }

        [Test]
        public void Should_sum_many_values_from_example_puzzle_input()
        {
            var examplePuzzleInput = @"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet";
            var calibration = TrebuchetCalibration.SumCalibration(examplePuzzleInput.Split(Environment.NewLine));
            Assert.That(calibration, Is.EqualTo(142));
        }

        [Test]
        public void Should_sum_many_values_from_puzzle_input_file()
        {
            var day1PuzzleInput = PuzzleInput.GetFile("day1.txt");
            var calibration = TrebuchetCalibration.SumCalibration(day1PuzzleInput);
            //Assert.That(calibration, Is.EqualTo(54916)); part 1
            Assert.That(calibration, Is.EqualTo(54728));
        }
    }
}