using BallSort.Engine.Game;
using BallSort.Engine.Models;
using Xunit;

namespace BallSort.Engine.Tests.Game;

public class BoardTests
{
    [Theory]
    // [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 4, 0, "Source column 4 is out of bounds")]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 0, 4, "Target column 4 is out of bounds")]
    public void MoveThrowsForInvalidMoves(string layout, int source, int target, string expectedMessage)
    {
        var board = BoardFromLayout(3, 4, layout);

        try
        {
            board.Move(source, target);
        }
        catch (Exception exception)
        {
            Assert.Equal(expectedMessage, exception.Message);
        }
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