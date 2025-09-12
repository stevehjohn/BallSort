using BallSort.Engine.Infrastructure;
using Xunit;

namespace BallSort.Engine.Tests.Infrastructure;

[CollectionDefinition("NonParallel", DisableParallelization = true)]
public class PuzzleManagerTests
{
    [Fact]
    public void GetPuzzleReturnsExpectedBoard()
    {
        PuzzleManager.Path = "Test Data/Puzzles.json";
        
        var board = PuzzleManager.Instance.GetPuzzle(0);
        
        Assert.Equal(8, board.Width);
        
        Assert.Equal(4, board.Height);
        
        Assert.Equal("DarkPurple,Red,Yellow,Green", string.Join(',', board.GetColumn(0)));
        
        Assert.Equal("Yellow,Green,Red,Orange", string.Join(',', board.GetColumn(1)));
        
        Assert.Equal("Blue,Blue,Blue,Red", string.Join(',', board.GetColumn(2)));
        
        Assert.Equal("DarkPurple,Orange,Orange,Green", string.Join(',', board.GetColumn(3)));
        
        Assert.Equal("DarkPurple,Yellow,Green,Blue", string.Join(',', board.GetColumn(4)));
        
        Assert.Equal("DarkPurple,Red,Yellow,Orange", string.Join(',', board.GetColumn(5)));
        
        Assert.Equal("Empty,Empty,Empty,Empty", string.Join(',', board.GetColumn(6)));
        
        Assert.Equal("Empty,Empty,Empty,Empty", string.Join(',', board.GetColumn(7)));
    }

    [Fact]
    public void GetPuzzleThrowsWhenPathNotSet()
    {
        PuzzleManager.Path = null;
        
        var exception = Assert.Throws<InvalidOperationException>(() => PuzzleManager.Instance.GetPuzzle(0));
        
        Assert.Equal("Please set the Path property before using the PuzzleManager.", exception.Message);
    }
}