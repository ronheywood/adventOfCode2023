using TestProject1.Helpers;

namespace TestProject1;

public class Day8
{
    [Test]
    public void grid_size_should_be_99_square()
    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        Assert.Multiple(() =>
        {
            Assert.That(forest.GridWidth, Is.EqualTo(99));
            Assert.That(forest.GridHeight, Is.EqualTo(99));
        });
    }

    [Test]
    public void Should_be_99x99_trees()
    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        Assert.That(forest.Trees.Count(), Is.EqualTo(99 * 99));
    }

    [TestCase(1805)]
    //4450 is too high
    public void Should_count_number_of_visible_trees(int correctAnswer)
    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        Assert.That(forest.VisibleTrees().Count(), Is.EqualTo(correctAnswer));
    }

    [TestCase(444528)]
    public void Should_get_the_highest_scenic_score(int correctAnswer)
    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        Assert.That(forest.MaxScenicScore(), Is.EqualTo(correctAnswer));
    }
}