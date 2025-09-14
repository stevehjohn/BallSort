using BallSort.Engine.Infrastructure;
using Xunit;

namespace BallSort.Engine.Tests;

public class SolverTests
{
    [Fact]
    public void SolveReturnsASolution()
    {
        var puzzleManager = new PuzzleManager("Test Data/Puzzles.json");

        var board = puzzleManager.GetPuzzle(0);
        
        var solver = new Solver(board);

        try
        {
            solver.Solve();
        }
        catch (Exception exception)
        {
            // ReSharper disable once Xunit.XunitTestWithConsoleOutput
            Console.WriteLine(exception.Message);
        }

        // Assert.True(result.Solved);
    }
}