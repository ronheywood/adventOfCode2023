using System.Collections;
using NUnit.Framework.Constraints;
using TestProject1.Helpers;

namespace TestProject1;

public class Day1
{
    public class ItCollatesCaloriesOfSnacksCarriedByElves
    {
        [Test]
        public void No_elves()
        {
            var input = Array.Empty<string>();
            var result = CalorieCounter.MostCalories(input);
            Assert.That(result,Is.EqualTo(0));
        }
        
        [Test]
        public void One_elf_one_snack()
        {
            var input = new string[]{"1"};
            var result = CalorieCounter.MostCalories(input);
            Assert.That(result,Is.EqualTo(1));
        }
        
        [Test]
        public void One_elf_two_snack()
        {
            var input = new string[]{"1","2"};
            var result = CalorieCounter.MostCalories(input);
            Assert.That(result,Is.EqualTo(3));
        }
        
        [Test]
        public void Two_elf_same_snack()
        {
            var input = new string[]{"1","","1"};
            var result = CalorieCounter.MostCalories(input);
            Assert.That(result,Is.EqualTo(1));
        }
        
        [Test]
        public void Two_elf_second_snack_has_more_calories()
        {
            var input = new string[]{"1","","2"};
            var result = CalorieCounter.MostCalories(input);
            Assert.That(result,Is.EqualTo(2));
        }
        
        [Test]
        public void Two_elf_first_snack_has_more_calories()
        {
            var input = new string[]{"2","","1"};
            var result = CalorieCounter.MostCalories(input);
            Assert.That(result,Is.EqualTo(2));
        }
        
        [Test]
        public void Two_elf_first_has_many_snacks()
        {
            var input = new string[]{"1","1","","1"};
            var result = CalorieCounter.MostCalories(input);
            Assert.That(result,Is.EqualTo(2));
        }
        
        [Test]
        public void Two_elf_both_have_many_snacks()
        {
            var input = new string[]{"1","2","","1","1"};
            var result = CalorieCounter.MostCalories(input);
            Assert.That(result,Is.EqualTo(3));
        }
        
        [TestCase(74198)]
        public void Puzzle_input_part_one(int correctAnswer)
        {
            var input = PuzzleInput.GetFile("day1.txt");
            var result = CalorieCounter.MostCalories(input);
            Assert.That(result,Is.EqualTo(correctAnswer));
        }

        [Test]
        public void Get_calories_ordered()
        {
            var input = new string[]{"1","2","","1","1"};
            var result = CalorieCounter.OrderedCalories(input);
            var expected = new [] { 3, 2 };
            CollectionAssert.AreEquivalent(expected,result);
        }
        
        [TestCase(209914)]
        public void Puzzle_input_part_two(int correctAnswer)
        {
            var input = PuzzleInput.GetFile("day1.txt");
            var result = CalorieCounter.OrderedCalories(input).Take(3).Sum();
            Assert.That(result,Is.EqualTo(correctAnswer));
        }
    }
}