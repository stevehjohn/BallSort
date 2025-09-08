using BallSort.Engine.Infrastructure;
using Xunit;

namespace BallSort.Engine.Tests.Infrastructure;

[CollectionDefinition("NonParallel", DisableParallelization = true)]
public class PuzzleManagerTests
{
    [Fact]
    public void GetPuzzleReturnsExpectedBoard()
    {
        PuzzleManager.Path = "Test Data/SmallPuzzle.json";
        
        var board = PuzzleManager.Instance.GetPuzzle(0);
        
        Assert.Equal(4, board.Width);
        
        Assert.Equal(3, board.Height);
        
        Assert.Equal("Red,Green,Yellow", string.Join(',', board.GetColumn(0)));
        
        Assert.Equal("Blue,Orange,DarkPurple", string.Join(',', board.GetColumn(1)));
        
        Assert.Equal("Empty,Empty,Empty", string.Join(',', board.GetColumn(2)));
        
        Assert.Equal("Empty,Empty,Empty", string.Join(',', board.GetColumn(3)));
    }
}