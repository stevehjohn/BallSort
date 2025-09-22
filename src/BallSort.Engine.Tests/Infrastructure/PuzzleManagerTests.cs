using BallSort.Engine.Infrastructure;
using Xunit;

namespace BallSort.Engine.Tests.Infrastructure;

[CollectionDefinition("NonParallel", DisableParallelization = true)]
public class PuzzleManagerTests
{
    [Fact]
    public void GetPuzzleReturnsExpectedBoard()
    {
        var puzzleManager = new PuzzleManager("Test Data/Puzzles.json");

        var board = puzzleManager.GetPuzzle(1);
        
        Assert.Equal(8, board.Width);
        
        Assert.Equal(4, board.Height);
        
        Assert.Equal("Yellow,Green,Orange,Yellow", string.Join(',', board.GetColumn(0).ToArray()));
        
        Assert.Equal("Green,Red,Yellow,Orange", string.Join(',', board.GetColumn(1).ToArray()));
        
        Assert.Equal("Blue,Green,DarkPurple,Blue", string.Join(',', board.GetColumn(2).ToArray()));
        
        Assert.Equal("DarkPurple,Blue,Red,Red", string.Join(',', board.GetColumn(3).ToArray()));
        
        Assert.Equal("Orange,Red,Yellow,DarkPurple", string.Join(',', board.GetColumn(4).ToArray()));
        
        Assert.Equal("DarkPurple,Orange,Blue,Green", string.Join(',', board.GetColumn(5).ToArray()));
        
        Assert.Equal("Empty,Empty,Empty,Empty", string.Join(',', board.GetColumn(6).ToArray()));
        
        Assert.Equal("Empty,Empty,Empty,Empty", string.Join(',', board.GetColumn(7).ToArray()));
    }

    [Fact]
    public void ConstructorThrowsWhenPathNotSet()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => new PuzzleManager(string.Empty));
        
        Assert.Equal("Please pass in a valid path.", exception.Message);
    }
}