using BallSort.Engine.Infrastructure;
using Xunit;

namespace BallSort.Engine.Tests;

public class SolverTests
{
    [Fact]
    public void SolveReturnsASolution()
    {
        var puzzleManager = new PuzzleManager("Test Data/Puzzles.json");

        for (var i = 0; i < puzzleManager.PuzzleCount; i++)
        {
            var board = puzzleManager.GetPuzzle(i);

            var solver = new Solver(board);

            var result = solver.Solve();

            Assert.True(result.Solved);
        }
    }
}