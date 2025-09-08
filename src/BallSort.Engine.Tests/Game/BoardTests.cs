using BallSort.Engine.Game;
using Xunit;

namespace BallSort.Engine.Tests.Game;

public class BoardTests
{
    [Theory]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 4, 0, "")]
    public void MoveThrowsForInvalidMoves(string layout, int source, int target, string expectedMessage)
    {
        var board = new Board();

        try
        {
            board.Move(source, target);
        }
        catch (Exception exception)
        {
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}