namespace TestProject1.Helpers;

public class ForestGridTests
{
    private const string SpecificationGrid = "30373\r\n25512\r\n65332\r\n33549\r\n35390";

    [TestCaseSource(nameof(ForestGridTestData))]
    public void Should_identify_trees_in_a_grid(string grid, int expectedCount, int expectedGridWidth,
        int expectedGridHeight)
    {
        var gridArray = PuzzleInput.InputStringToArray(grid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.Multiple(() =>
        {
            Assert.That(forestGrid.Trees, Has.Count.EqualTo(expectedCount));
            Assert.That(forestGrid.GridWidth, Is.EqualTo(expectedGridWidth));
            Assert.That(forestGrid.GridHeight, Is.EqualTo(expectedGridHeight));
        });
    }

    [TestCase(0, 0)]
    [TestCase(1, 0)]
    [TestCase(2, 0)]
    [TestCase(3, 0)]
    [TestCase(4, 0)]
    public void The_first_row_of_trees_is_visible_as_no_trees_are_between_it_and_the_top_edge_of_the_grid(int x, int y)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(x, y), Is.True);
    }
    
    [TestCase(0,0)]
    [TestCase(0,1)]
    [TestCase(0,2)]
    [TestCase(0,3)]
    [TestCase(0,4)]
    public void The_first_column_of_trees_is_visible_as_no_trees_are_between_it_and_the_left_edge_of_the_grid(int x, int y)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(x, y), Is.True);
    }

    [TestCase(0, 4)]
    [TestCase(1, 4)]
    [TestCase(2, 4)]
    [TestCase(3, 4)]
    [TestCase(4, 4)]
    public void The_last_row_of_trees_is_visible_as_no_trees_are_between_it_and_the_bottom_edge_of_the_grid(int x,
        int y)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(x, y), Is.True);
    }

    [TestCase(4, 0)]
    [TestCase(4, 1)]
    [TestCase(4, 2)]
    [TestCase(4, 3)]
    [TestCase(4, 4)]
    public void The_last_column_of_trees_is_visible_as_no_trees_are_between_it_and_the_right_edge_of_the_grid(int x,
        int y)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(x, y), Is.True);
    }

    [Test]
    public void Tree_1x1_with_height_5_is_visible_from_left_and_top()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        
        Assert.That(forestGrid.IsTreeVisible(1, 1), Is.True);
    }

    [Test]
    public void Tree_1x2_with_height_5_is_visible_from_top_and_right()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(1, 2), Is.True);
    }

    [Test]
    public void Tree_1x3_with_height_1_is_not_visible_from_any_direction()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(1, 3), Is.False);
    }
    
    [Test]
    public void Tree_2x1_with_height_5_is_visible_from_the_east()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(2, 1), Is.True);
    }
    
    [Test]
    public void Tree_2x2_with_height_2_is_not_visible()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(2, 2), Is.False);
    }
    
    [Test]
    public void Tree_2x3_with_height_3_is_visible_from_the_east()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(2, 3), Is.True);
    }
    
    [Test]
    public void Tree_2x3_with_height_5_is_visible_from_the_west()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(2, 3), Is.True);
    }
    
    [Test]
    public void Tree_1x3_with_height_3_is_not_visible()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(1, 3), Is.False);
        
    }
    
    [Test]
    public void Tree_3x3_with_height_4_is_not_visible()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(3, 3), Is.False);
    }

    [Test]
    public void twenty_one_trees_are_visible_in_total()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.VisibleTrees().Count, Is.EqualTo(21));
    }

    [TestCase(0, 0, 0)]
    [TestCase(1, 0, 1)]
    [TestCase(2, 0, 2)]
    [TestCase(3, 0, 3)]
    [TestCase(4, 0, 4)]
    [TestCase(0, 1, 5)]
    [TestCase(1, 1, 6)]
    [TestCase(2, 1, 7)]
    [TestCase(3, 1, 8)]
    [TestCase(4, 1, 9)]
    public void Should_get_tree_by_grid_reference_starting_0_0(int x, int y, int expected)
    {
        var gridArray = PuzzleInput.InputStringToArray("01234\r\n56789");
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.GetTree(x, y), Is.EqualTo(expected));
    }

    [TestCase(0, 2, 0)]
    [TestCase(1, 2, 1)]
    [TestCase(2, 2, 2)]
    [TestCase(3, 2, 3)]
    [TestCase(4, 2, 4)]
    [TestCase(0, 3, 5)]
    [TestCase(1, 3, 6)]
    [TestCase(2, 3, 7)]
    [TestCase(3, 3, 8)]
    [TestCase(4, 3, 9)]
    public void Should_get_tree_by_grid_reference_starting_0_2(int x, int y, int expected)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00000\r\n01234\r\n56789");
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.GetTree(x, y), Is.EqualTo(expected));
    }

    [Test]
    public void Should_get_tree_south_east_corner()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00000\r\n00000\r\n00000\r\n01234");
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.GetTree(4, 4), Is.EqualTo(4));
    }

    [Test]
    public void Should_find_no_trees_north_of_row_1([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00000\r\n01234\r\n56789");
        var forestGrid = new ForestGrid(gridArray);
        var tree = forestGrid.GetTree(x, 0);
        Assert.That(forestGrid.TreesNorth(x, 0), Is.Empty);
    }

    [Test]
    public void Should_find_1_trees_north_of_row_1([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray("01234\r\n56789");
        var expectedTree = x;
        var forestGrid = new ForestGrid(gridArray);

        CollectionAssert.AreEqual(new[] { expectedTree }, forestGrid.TreesNorth(x, 1));
    }

    [Test]
    public void Should_find_2_trees_north_of_row_2([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray("01234\r\n01234\r\n00000");
        var expectedTree = x;
        var forestGrid = new ForestGrid(gridArray);

        CollectionAssert.AreEqual(new[] { expectedTree, expectedTree }, forestGrid.TreesNorth(x, 2));
    }

    [Test]
    public void Should_find_no_trees_south_of_row_4([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);

        Assert.That(forestGrid.TreesSouth(x, 4), Is.Empty);
    }

    [Test]
    public void Should_find_1_trees_south_of_row_1([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n01234");
        var expectedTree = x;
        var forestGrid = new ForestGrid(gridArray);

        CollectionAssert.AreEqual(new[] { expectedTree }, forestGrid.TreesSouth(x, 0));
    }


    [Test]
    public void Should_find_2_trees_south_of_row_1([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n01234\r\n01234");
        var expectedTree = x;
        var forestGrid = new ForestGrid(gridArray);

        CollectionAssert.AreEqual(new[] { expectedTree, expectedTree }, forestGrid.TreesSouth(x, 0));
    }

    [Test]
    public void Should_find_no_trees_west_of_column_0([Values(0, 1, 2, 3, 4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);

        Assert.That(forestGrid.TreesWest(0, y), Is.Empty);
    }

    [Test]
    public void Should_find_1_tree_west_of_column_1([Values(0, 1, 2, 3, 4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n10000\r\n20000\r\n30000\r\n40000");
        var forestGrid = new ForestGrid(gridArray);

        var expectedTree = y;
        CollectionAssert.AreEqual(new[] { expectedTree }, forestGrid.TreesWest(1, y));
    }

    [Test]
    public void Should_find_2_tree_west_of_column_2([Values(0, 1, 2, 3, 4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n11000\r\n22000\r\n33000\r\n44000");
        var forestGrid = new ForestGrid(gridArray);

        var expectedTree = y;
        CollectionAssert.AreEqual(new[] { expectedTree, expectedTree }, forestGrid.TreesWest(2, y));
    }


    [Test]
    public void Should_find_no_trees_east_of_column_4([Values(0, 1, 2, 3, 4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);

        Assert.That(forestGrid.TreesEast(4, y), Is.Empty);
    }
    
    
    [Test]
    public void Should_find_one_tree_east_of_column_3([Values(0,1,2,3,4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00001\r\n00002\r\n00003\r\n00004");
        var forestGrid = new ForestGrid(gridArray);
        
        var expectedTree = y;
        CollectionAssert.AreEqual(new[] { expectedTree }, forestGrid.TreesEast(3, y));
    }
    
    [Test]
    public void Should_find_two_trees_east_of_column_2([Values(0,1,2,3,4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var forestGrid = new ForestGrid(gridArray);
        
        var expectedTree = y;
        CollectionAssert.AreEqual(new[] { expectedTree,expectedTree }, forestGrid.TreesEast(2, y));
    }

    [Test]
    public void Should_identifY_if_a_tree_is_visible()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(0,0), Is.True);
    }

    private static IEnumerable<object[]> ForestGridTestData()
    {
        yield return new object[] { "0", 1, 1, 1 };
        yield return new object[] { "30373", 5, 5, 1 };
        yield return new object[] { "30373\r\n25512", 10, 5, 2 };
        yield return new object[] { "30373\r\n25512\r\n65332", 15, 5, 3 };
        yield return new object[] { "30373\r\n25512\r\n65332\r\n33549", 20, 5, 4 };
        yield return new object[] { SpecificationGrid, 25, 5, 5 };
    }
}