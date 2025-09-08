using BallSort.Engine.Exceptions;
using BallSort.Engine.Game;
using BallSort.Engine.Models;
using Xunit;

namespace BallSort.Engine.Tests.Game;

public class BoardTests
{
    [Theory]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 4, 0, "Source column 4 is out of bounds.")]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 0, 4, "Target column 4 is out of bounds.")]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", -1, 0, "Source column -1 is out of bounds.")]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 0, -4, "Target column -4 is out of bounds.")]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 3, 40, "Source column 3 contains no balls.")]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 0, 1, "Target column 1 is full.")]
    [InlineData("1,2,3,1,4,0,0,0,0,0,0,0", 0, 1, "Cannot move Yellow ball from column 0 onto Blue ball in column 1.")]
    [InlineData("1,2,3,1,3,0,0,0,0,0,0,0", 0, 1, null)]
    public void MoveThrowsForInvalidMoves(string layout, int source, int target, string expectedMessage)
    {
        var board = BoardFromLayout(4, 3, layout);

        try
        {
            board.Move(source, target);
        }
        catch (Exception exception)
        {
            Assert.Equal(expectedMessage, exception.Message);
            
            return;
        }
        
        Assert.Null(expectedMessage);
    }

    [Theory]
    [InlineData("1,2,3,5,6,7,0,0,0,0,0,0", 0, "Red,Green,Yellow")]
    [InlineData("1,2,3,5,6,7,0,0,0,0,0,0", 2, "Empty,Empty,Empty")]
    public void GetColumnReturnsCorrectData(string layout, int column, string expected)
    {
        var board = BoardFromLayout(4, 3, layout);
        
        Assert.Equal(expected, string.Join(',', board.GetColumn(column)));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(2, false)]
    [InlineData(3, false)]
    public void IsCompleteReturnsCorrectValue(int column, bool isComplete)
    {
        var board = BoardFromLayout(4, 3, "1,2,3,4,4,4,5,6,0,0,0,0");
        
        Assert.Equal(isComplete, board.IsComplete(column));
    }
    
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(3, true)]
    public void IsEmptyReturnsCorrectValue(int column, bool isComplete)
    {
        var board = BoardFromLayout(4, 3, "1,2,3,4,4,4,5,6,0,0,0,0");
        
        Assert.Equal(isComplete, board.IsEmpty(column));
    }

    [Fact]
    public void GetColumnThrowsForInvalidColumn()
    {
        var board = BoardFromLayout(4, 3, "1,2,3,5,6,7,0,0,0,0,0,0");

        var exception = Assert.Throws<OutOfBoundsException>(() => board.GetColumn(4));
        
        Assert.Equal("Column 4 is out of bounds.", exception.Message);
    }
    
    [Fact]
    public void CloneCreatesACorrectCopy()
    {
        var board = BoardFromLayout(4, 3, "1,2,3,4,5,6,0,0,0,0,0,0");

        var clone = board.Clone();
        
        Assert.NotSame(board, clone);
        
        Assert.Equal(4, clone.Width);
        
        Assert.Equal(3, clone.Height);

        Assert.Equal("Red,Green,Yellow", string.Join(',', clone.GetColumn(0)));

        Assert.Equal("Blue,Orange,DarkPurple", string.Join(',', clone.GetColumn(1)));

        Assert.Equal("Empty,Empty,Empty", string.Join(',', clone.GetColumn(2)));

        Assert.Equal("Empty,Empty,Empty", string.Join(',', clone.GetColumn(3)));
        
        clone.Move(0, 2);
        
        Assert.Equal("Red,Green,Yellow", string.Join(',', board.GetColumn(0)));
        
        Assert.Equal("Empty,Empty,Empty", string.Join(',', board.GetColumn(2)));
        
        Assert.Equal("Red,Green,Empty", string.Join(',', clone.GetColumn(0)));
        
        Assert.Equal("Yellow,Empty,Empty", string.Join(',', clone.GetColumn(2)));
    }

    [Theory]
    [InlineData(Colour.Yellow, 0)]
    [InlineData(Colour.DarkPurple, 1)]
    [InlineData(Colour.Empty, 2)]
    [InlineData(Colour.Empty, 3)]
    public void TopReturnsCorrectColour(Colour expectedColour, int column)
    {
        var board = BoardFromLayout(4, 3, "1,2,3,4,5,6,0,0,0,0,0,0");
        
        Assert.Equal(expectedColour, board.Top(column));
    }

    [Theory]
    [InlineData(true, 0)]
    [InlineData(true, 1)]
    [InlineData(false, 2)]
    [InlineData(false, 3)]
    public void IsFullReturnsCorrectValue(bool isFull, int column)
    {
        var board = BoardFromLayout(4, 3, "1,2,3,4,5,6,0,0,0,0,0,0");
        
        Assert.Equal(isFull, board.IsFull(column));
    }

    [Theory]
    [InlineData(3, 0)]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    [InlineData(0, 3)]
    public void TopRunLengthReturnsCorrectValue(int expectedLength, int column)
    {
        var board = BoardFromLayout(4, 3, "1,1,1,2,3,3,4,4,5,0,0,0");
        
        Assert.Equal(expectedLength, board.TopRunLength(column));
    }

    private static Board BoardFromLayout(int width, int height, string layout)
    {
        var puzzle = new Puzzle
        {
            GridWidth = width,
            GridHeight = height,
            Data = new Data
            {
                Layout = new int[width * height]
            }
        };

        var balls = layout.Split(',');

        for (var i = 0; i < balls.Length; i++)
        {
            puzzle.Data.Layout[i] = int.Parse(balls[i]);
        }

        return new Board(puzzle);
    }
}