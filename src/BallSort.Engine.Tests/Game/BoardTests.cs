using BallSort.Engine.Game;
using BallSort.Engine.Models;
using Xunit;

namespace BallSort.Engine.Tests.Game;

public class BoardTests
{
    [Theory]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 4, 0, "Source column 4 is out of bounds.")]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 0, 4, "Target column 4 is out of bounds.")]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 3, 0, "Source column 3 contains no balls.")]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 0, 1, "Target column 1 is full.")]
    [InlineData("1,2,3,1,4,0,0,0,0,0,0,0", 0, 1, "Cannot move Yellow ball from column 0 onto Blue ball in column 1.")]
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