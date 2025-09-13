using BallSort.Engine.Infrastructure;
using Xunit;

namespace BallSort.Engine.Tests;

public class SolverTests
{
    [Fact]
    public void TestStub()
    {
        var puzzleManager = new PuzzleManager("Test Data/Puzzles.json");

        var board = puzzleManager.GetPuzzle(0);
        
        var solver = new Solver(board);

        var result = solver.Solve();
        
        // Assert.True(result.Solved);
    }
}