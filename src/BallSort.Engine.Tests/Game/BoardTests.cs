using Xunit;

namespace BallSort.Engine.Tests.Game;

public class BoardTests
{
    [Theory]
    [InlineData("1,1,1,1,1,1,0,0,0,0,0,0", 4, 0, "")]
    public void MoveThrowsForInvalidMoves(string layout, int source, int target, string exception)
    {
        
    }
}