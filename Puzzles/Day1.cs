using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;
using TestProject1.Helpers;

namespace TestProject1;

public class Day1
{
    public class TrebuchetCalibrationShould
    {
        // [TestCase("two1nine","219")]
        // [TestCase("eightwothree","823")]
        // [TestCase("abcone2threexyz","123")]
        // [TestCase("xtwone3four","234")]
        // [TestCase("zoneight234","18234")]
        // [TestCase("7pqrstsixteen","76")]
        // [TestCase("4nineeightseven2","42")]
        // public void Should_convert_natural_language_numbers_to_numeric(string example, string expected)
        // {
        //     var actual = TrebuchetCalibration.ReplaceWords(example);
        //     Assert.That(actual,Is.EqualTo(expected));
        // }

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
            var examplePuzzleInput = @"""1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet""";
            var calibration = TrebuchetCalibration.SumCalibration(examplePuzzleInput.Split(Environment.NewLine));
            Assert.That(calibration, Is.EqualTo(142));
        }

        [Test]
        public void Should_sum_many_values_from_puzzle_input_file()
        {
            var day1PuzzleInput = PuzzleInput.GetFile("day1.txt");
            var calibration = TrebuchetCalibration.SumCalibration(day1PuzzleInput);
            Assert.That(calibration, Is.EqualTo(54916));
        }
    }
}

public class TrebuchetCalibration
{
    private static Dictionary<string, string> _lookup = new Dictionary<string, string>()
    {
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" }
    };

    public static string Calibrate(string input)
    {
        
        return FirstWordOrNumber(input) + LastWordOrNumber(input);
        
    }

    public static int SumCalibration(IEnumerable<string> lines)
    {
        var numbers = lines.Select(Calibrate).ToList();
        if (numbers.Any(number => !int.TryParse(number,out _)))
        {
            throw new Exception($"A line has no numbers");
        }
        return numbers.Select(int.Parse).Sum();
    }

    public static string FirstWordOrNumber(string line)
    {
        if (line == string.Empty) return string.Empty;
        var firstNumber = string.Empty;
        if(_lookup.ContainsValue(line[0].ToString()))
        {
            return line[0].ToString();
        }

        foreach (var kvp in _lookup)
        {
            if (line.StartsWith(kvp.Key)) return kvp.Value;
        }
        return FirstWordOrNumber(line.Substring(1,line.Length-1));
    }

    public static string LastWordOrNumber(string line)
    {
        if (string.IsNullOrEmpty(line)) return string.Empty;
        if(_lookup.ContainsValue(line[^1].ToString()))
        {
            return line[^1].ToString();
        }

        foreach (var kvp in _lookup)
        {
            if (line.EndsWith(kvp.Key)) return kvp.Value;
        }
        return LastWordOrNumber(line.Substring(0,line.Length-1));
    }
}