namespace TestProject1.Helpers.Tests;

public class ForestGridTests
{
    private const string SpecificationGrid = "30373\r\n25512\r\n65332\r\n33549\r\n35390";

    [TestCaseSource(nameof(ForestGridTestData))]
    public void Should_identify_trees_in_a_grid(string grid, int expectedCount, int expectedGridWidth,
        int expectedGridHeight)
    {
        var gridArray = PuzzleInput.InputStringToArray(grid);
        var forestGrid = new IntPuzzleGrid(gridArray);
        Assert.Multiple(() =>
        {
            Assert.That(forestGrid.Items, Has.Count.EqualTo(expectedCount));
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
        var forestGrid = new ForestPuzzleGrid(gridArray);
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
        var forestGrid = new ForestPuzzleGrid(gridArray);
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
        var forestGrid = new ForestPuzzleGrid(gridArray);
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
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(x, y), Is.True);
    }

    [Test]
    public void Tree_1x1_with_height_5_is_visible_from_left_and_top()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        
        Assert.That(forestGrid.IsTreeVisible(1, 1), Is.True);
    }

    [Test]
    public void Tree_1x2_with_height_5_is_visible_from_top_and_right()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(1, 2), Is.True);
    }

    [Test]
    public void Tree_1x3_with_height_1_is_not_visible_from_any_direction()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(1, 3), Is.False);
    }
    
    [Test]
    public void Tree_2x1_with_height_5_is_visible_from_the_east()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(2, 1), Is.True);
    }
    
    [Test]
    public void Tree_2x2_with_height_2_is_not_visible()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(2, 2), Is.False);
    }
    
    [Test]
    public void Tree_2x3_with_height_3_is_visible_from_the_east()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(2, 3), Is.True);
    }
    
    [Test]
    public void Tree_2x3_with_height_5_is_visible_from_the_west()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(2, 3), Is.True);
    }
    
    [Test]
    public void Tree_1x3_with_height_3_is_not_visible()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(1, 3), Is.False);
        
    }
    
    [Test]
    public void Tree_3x3_with_height_4_is_not_visible()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(3, 3), Is.False);
    }

    [Test]
    public void twenty_one_trees_are_visible_in_total()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
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
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.GetItem(x, y), Is.EqualTo(expected));
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
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.GetItem(x, y), Is.EqualTo(expected));
    }

    [Test]
    public void Should_get_tree_south_east_corner()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00000\r\n00000\r\n00000\r\n01234");
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.GetItem(4, 4), Is.EqualTo(4));
    }
    
    [Test]
    public void Should_identify_if_a_tree_is_visible()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.IsTreeVisible(0,0), Is.True);
    }

    [TestCase(0,0,0,2, 2,0)]
    [TestCase(0,1,1,1, 1,0)]
    [TestCase(0,2,2,2, 4,0)]
    [TestCase(0,3,1,1, 1,0)]
    [TestCase(0,4,1,0, 1,0)]
    [TestCase(2,0,0,1, 1,2)]
    
    public void Should_identify_how_many_trees_in_each_direction_until_view_is_blocked_by_a_tree_same_height_or_higher(int x,int y,int viewingDistanceNorth,int viewingDistanceSouth, int viewingDistanceEast, int viewingDistanceWest)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.Multiple(() =>
        {
            Assert.That(forestGrid.ViewingDistanceNorth(x, y), Is.EqualTo(viewingDistanceNorth),"Viewing distance north was not matched");
            Assert.That(forestGrid.ViewingDistanceSouth(x, y), Is.EqualTo(viewingDistanceSouth),"Viewing distance south was not matched");
            Assert.That(forestGrid.ViewingDistanceEast(x, y), Is.EqualTo(viewingDistanceEast),"Viewing distance east was not matched");
            Assert.That(forestGrid.ViewingDistanceWest(x, y), Is.EqualTo(viewingDistanceWest),"Viewing distance wast was not matched");
        });
    }

    [TestCase(0,2,2)]
    public void the_tree_that_blocks_view_is_included_in_the_count(int x,int y,int viewingDistance)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);
        Assert.That(forestGrid.ViewingDistanceNorth(x,y), Is.EqualTo(viewingDistance));
    }

    [Test]
    public void Scenic_score_is_the_product_of_all_visible_trees_in_each_direction()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);

        Assert.That(forestGrid.ScenicScore(2, 1), Is.EqualTo(4));
    }

    [Test]
    public void max_scenic_score()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new ForestPuzzleGrid(gridArray);

        Assert.That(forestGrid.MaxScenicScore(), Is.EqualTo(8));
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

public class GridCompassTests
{
    private GridCompass _gridCompass = null!;
    private const string SpecificationGrid = "30373\r\n25512\r\n65332\r\n33549\r\n35390";
    
    [Test]
    public void Should_find_no_trees_north_of_row_1([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00000\r\n01234\r\n56789");
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);
        Assert.That(_gridCompass.AllItemsNorth(x, 0), Is.Empty);
    }

    [Test]
    public void Should_find_1_trees_north_of_row_1([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray("01234\r\n56789");
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);

        CollectionAssert.AreEqual(new[] { x }, _gridCompass.AllItemsNorth(x, 1));
    }

    [Test]
    public void Should_find_2_trees_north_of_row_2([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray("12345\r\n01234\r\n00000");
        var expectedTree = x;
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);

        CollectionAssert.AreEqual(new[] { expectedTree, expectedTree+1 }, _gridCompass.AllItemsNorth(x, 2));
    }

    [Test]
    public void Should_find_no_trees_south_of_row_4([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);

        Assert.That(_gridCompass.AllItemsSouth(x, 4), Is.Empty);
    }

    [Test]
    public void Should_find_1_trees_south_of_row_1([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n01234");
        var expectedTree = x;
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);

        CollectionAssert.AreEqual(new[] { expectedTree }, _gridCompass.AllItemsSouth(x, 0));
    }


    [Test]
    public void Should_find_2_trees_south_of_row_1([Values(0, 1, 2, 3, 4)] int x)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n01234\r\n12345");
        var expectedTree = x;
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);

        CollectionAssert.AreEqual(new[] { expectedTree, expectedTree+1 }, _gridCompass.AllItemsSouth(x, 0));
    }

    [Test]
    public void Should_find_no_trees_west_of_column_0([Values(0, 1, 2, 3, 4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);

        Assert.That(_gridCompass.AllItemsWest(0, y), Is.Empty);
    }

    [Test]
    public void Should_find_1_tree_west_of_column_1([Values(0, 1, 2, 3, 4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n10000\r\n20000\r\n30000\r\n40000");
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);

        var expectedTree = y;
        CollectionAssert.AreEqual(new[] { expectedTree }, _gridCompass.AllItemsWest(1, y));
    }
    [Test]
    public void Should_find_2_tree_west_of_column_2([Values(0, 1, 2, 3, 4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n11000\r\n22000\r\n33000\r\n44000");
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);

        var expectedTree = y;
        CollectionAssert.AreEqual(new[] { expectedTree, expectedTree }, _gridCompass.AllItemsWest(2, y));
    }

    [Test]
    public void Specification_grid_test()
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);
        Assert.Multiple(() =>
        {
            CollectionAssert.AreEqual(new[] { 0, 3 }, _gridCompass.AllItemsWest(2, 0));
            CollectionAssert.AreEqual(new[] { 5, 3 }, _gridCompass.AllItemsNorth(2, 2));
            CollectionAssert.AreEqual(new[] { 5, 3 }, _gridCompass.AllItemsSouth(2, 2));
            CollectionAssert.AreEqual(new[] { 3, 2 }, _gridCompass.AllItemsEast(2, 2));
        });
    }


    [Test]
    public void Should_find_no_trees_east_of_column_4([Values(0, 1, 2, 3, 4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray(SpecificationGrid);
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);

        Assert.That(_gridCompass.AllItemsEast(4, y), Is.Empty);
    }

    [Test]
    public void Should_get_item_at_coordinates()
    {
        var gridArray = PuzzleInput.InputStringToArray("01234\r\n56789");
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);
        Assert.That(_gridCompass.GetItem(0,0),Is.EqualTo("0"));
        Assert.That(_gridCompass.GetItem(1,0),Is.EqualTo("1"));
        Assert.That(_gridCompass.GetItem(3,0),Is.EqualTo("3"));
        Assert.That(_gridCompass.GetItem(4,0),Is.EqualTo("4"));
        
        Assert.That(_gridCompass.GetItem(0,1),Is.EqualTo("5"));
        
    }
    
    
    [Test]
    public void Should_find_one_tree_east_of_column_3([Values(0,1,2,3,4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00001\r\n00002\r\n00003\r\n00004");
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);
        
        var expectedTree = y;
        CollectionAssert.AreEqual(new[] { expectedTree }, _gridCompass.AllItemsEast(3, y));
    }
    
    [Test]
    public void Should_find_two_trees_east_of_column_2([Values(0,1,2,3,4)] int y)
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var forestGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new ForestGridCompass(forestGrid);
        
        var expectedTree = y;
        CollectionAssert.AreEqual(new[] { expectedTree,expectedTree }, _gridCompass.AllItemsEast(2, y));
    }

    [Test]
    public void Should_find_no_neighbor_north_west_of_column_0()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.That(_gridCompass.NorthWestNeighbor(0, 0), Is.Null);
    }
    
    [Test]
    public void Should_find_neighbor_north()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.Multiple(() =>
        {
            Assert.That(_gridCompass.NorthNeighbor(4, 4), Is.EqualTo("3"));
            Assert.That(_gridCompass.NorthNeighbor(4, 3), Is.EqualTo("2"));
            Assert.That(_gridCompass.NorthNeighbor(4, 2), Is.EqualTo("1"));
            Assert.That(_gridCompass.NorthNeighbor(4, 1), Is.EqualTo("0"));
        });
    }

    [Test]
    public void Should_find_no_neighbor_north_of_row_0()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.That(_gridCompass.NorthNeighbor(0, 0), Is.Null);
    }
    
    [Test]
    public void Should_find_no_neighbor_south_of_row_5()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.That(_gridCompass.SouthNeighbor(4, 4), Is.Null);
    }
    
    [Test]
    public void Should_find_neighbor_south()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.Multiple(() =>
        {
            Assert.That(_gridCompass.SouthNeighbor(4, 0), Is.EqualTo("1"));
            Assert.That(_gridCompass.SouthNeighbor(4, 1), Is.EqualTo("2"));
            Assert.That(_gridCompass.SouthNeighbor(4, 2), Is.EqualTo("3"));
            Assert.That(_gridCompass.SouthNeighbor(4, 3), Is.EqualTo("4"));
        });
    }

    [Test]
    public void Should_find_no_items_south_west_of_column_0()
    { var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.That(_gridCompass.SouthWestNeighbor(0, 0), Is.Null);
    }
    
    
    [Test]
    public void Should_find_no_items_west_of_column_0()
    { var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.That(_gridCompass.WestNeighbor(0, 4), Is.Null);
    }
    
    [Test]
    public void Should_find_items_west_of_column_5()
    { var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.That(_gridCompass.WestNeighbor(4, 4), Is.EqualTo("4"));
        Assert.That(_gridCompass.WestNeighbor(4, 3), Is.EqualTo("3"));
        Assert.That(_gridCompass.WestNeighbor(4, 2), Is.EqualTo("2"));
        Assert.That(_gridCompass.WestNeighbor(4, 1), Is.EqualTo("1"));
    }
    
    [Test]
    public void Should_find_items_east_of_column_0()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.Multiple(() =>
        {
            Assert.That(_gridCompass.EastNeighbor(2, 0), Is.EqualTo("0"));
            Assert.That(_gridCompass.EastNeighbor(2, 1), Is.EqualTo("1"));
            Assert.That(_gridCompass.EastNeighbor(2, 2), Is.EqualTo("2"));
            Assert.That(_gridCompass.EastNeighbor(2, 3), Is.EqualTo("3"));
            Assert.That(_gridCompass.EastNeighbor(2, 4), Is.EqualTo("4"));
        });
    }

    [Test]
    public void Should_find_no_items_north_east_of_column_4()
    { var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.That(_gridCompass.NorthEastNeighbor(4, 0), Is.Null);
    }
    
    [Test]
    public void Should_find_item_north_east_of_column_3()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.Multiple(() =>
        {
            Assert.That(_gridCompass.NorthEastNeighbor(3, 4), Is.EqualTo("3"));
            Assert.That(_gridCompass.NorthEastNeighbor(3, 3), Is.EqualTo("2"));
            Assert.That(_gridCompass.NorthEastNeighbor(3, 2), Is.EqualTo("1"));
            Assert.That(_gridCompass.NorthEastNeighbor(3, 1), Is.EqualTo("0"));
        });
    }
    
    [Test]
    public void Should_find_item_north_west_of_column_4()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.Multiple(() =>
        {
            Assert.That(_gridCompass.NorthWestNeighbor(4, 4), Is.EqualTo("3"));
            Assert.That(_gridCompass.NorthWestNeighbor(4, 3), Is.EqualTo("2"));
            Assert.That(_gridCompass.NorthWestNeighbor(4, 2), Is.EqualTo("1"));
            Assert.That(_gridCompass.NorthWestNeighbor(4, 1), Is.EqualTo("0"));
        });
    }

    [Test]
    public void Should_find_no_items_south_east_of_column_4()
    { var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.That(_gridCompass.SouthEastNeighbor(4, 0), Is.Null);
    }
    
    [Test]
    public void Should_find_no_items_east_of_column_4()
    { var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.That(_gridCompass.EastNeighbor(4, 0), Is.Null);
    }
    
    [Test]
    public void Should_find_items_south_east_of_column_3()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.Multiple(() =>
        {
            Assert.That(_gridCompass.SouthEastNeighbor(3, 0), Is.EqualTo("1"));
            Assert.That(_gridCompass.SouthEastNeighbor(3, 1), Is.EqualTo("2"));
            Assert.That(_gridCompass.SouthEastNeighbor(3, 2), Is.EqualTo("3"));
            Assert.That(_gridCompass.SouthEastNeighbor(3, 3), Is.EqualTo("4"));
        });
    }
    
    [Test]
    public void Should_find_items_south_west_of_column_4()
    {
        var gridArray = PuzzleInput.InputStringToArray("00000\r\n00011\r\n00022\r\n00033\r\n00044");
        var puzzleGrid = new IntPuzzleGrid(gridArray);
        _gridCompass = new GridCompass(puzzleGrid);
        Assert.Multiple(() =>
        {
            Assert.That(_gridCompass.SouthWestNeighbor(4, 0), Is.EqualTo("1"));
            Assert.That(_gridCompass.SouthWestNeighbor(4, 1), Is.EqualTo("2"));
            Assert.That(_gridCompass.SouthWestNeighbor(4, 2), Is.EqualTo("3"));
            Assert.That(_gridCompass.SouthWestNeighbor(4, 3), Is.EqualTo("4"));
        });
    }
}