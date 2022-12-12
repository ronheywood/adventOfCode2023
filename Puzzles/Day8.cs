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

    [Test]
    public void Should_be_no_trees_above_row_1([Range(0, 99)] int x)

    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        var y = 0;
        Assert.That(forest.TreesNorth(x,y).Count(), Is.EqualTo(0));
    }
    
    [Test]
    public void Should_be_no_trees_below_row_99([Range(0, 99)] int x)

    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        var y = 99;
        Assert.That(forest.TreesSouth(x,y).Count(), Is.EqualTo(0));
    }
    
    [Test]
    public void Should_be_no_trees_west_column_0([Range(0, 99)] int y)
    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        var x = 0;
        Assert.That(forest.TreesWest(x,y).Count(), Is.EqualTo(0));
    }
    
    [Test]
    public void Should_be_no_trees_east_column_99([Range(0, 99)] int y)
    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        var x = 99;
        Assert.That(forest.TreesEast(x,y).Count(), Is.EqualTo(0));
    }
    
    
    [Test]
    public void Tree_31x44_should_be_9_and_visible()
    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        Assert.Multiple(() =>
        {
            Assert.That(forest.GetTree(31, 44), Is.EqualTo(9));
            Assert.That(forest.IsTreeVisible(31, 44), Is.True);
        });
    }
    
    [Test]
    public void Tree_30x44_should_be_8_and_not_visible()
    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        Assert.Multiple(() =>
        {   //1,31
            Assert.That(forest.GetTree(30, 44), Is.EqualTo(8));
            
            Assert.That(forest.VisibleNorth(30, 44), Is.False,"Should not be Visible from north");
            Assert.That(forest.VisibleSouth(30, 44), Is.False,"Should not be Visible from south");
            Assert.That(forest.VisibleEast(30, 44), Is.False,"Should not be Visible from east");
            
            Assert.That(forest.TreesWest(30, 44).Count(), Is.EqualTo(30),"30 trees to the west");
            CollectionAssert.AreEqual(new[]{5,3,1,5,3,2,4,1,4,2,2,3,2,4,3,4,4,5,4,6,6,3,4,8,8,4,8,5,7,5},forest.TreesWest(30,44));
            Assert.That(forest.VisibleWest(30, 44), Is.False,"Should not be Visible from wast");
            
            Assert.That(forest.IsTreeVisible(30, 44), Is.False, "should not be visible at all");
        });
    }

    [TestCase(1805)]
    //4450 is too high
    public void Should_count_number_of_visible_trees(int correctAnswer)
    {
        var input = PuzzleInput.GetFile("day8.txt");
        var forest = new ForestGrid(input);
        Assert.That(forest.VisibleTrees().Count(), Is.EqualTo(correctAnswer));
    }
}