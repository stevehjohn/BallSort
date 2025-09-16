using System.Text;
using BallSort.Engine.Game;
using BallSort.Engine.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace BallSort.Engine.Tests;

public class SolverTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SolverTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void SolveReturnsASolution()
    {
        var puzzleManager = new PuzzleManager("Test Data/Puzzles.json");

        for (var i = 0; i < puzzleManager.PuzzleCount; i++)
        {
            var board = puzzleManager.GetPuzzle(i);
            
            DumpBoard(board);

            var solver = new Solver(board);

            var result = solver.Solve();

            Assert.True(result.Solved);
            
            DumpBoard(board);
            
            _testOutputHelper.WriteLine($"  Steps: {result.Moves.Count}.");
        }
    }

    private void DumpBoard(Board board)
    {
        Console.WriteLine();
        
        var builder = new StringBuilder();

        var columns = new List<Colour[]>();
        
        for (var x = 0; x < board.Width; x++)
        {
            columns.Add(board.GetColumn(x));
        }

        for (var y = 0; y < board.Height; y++)
        {
            for (var x = 0; x < board.Width; x++)
            {
                builder.Append((char) ('@' + (int) columns[x][y]));
            }

            _testOutputHelper.WriteLine($"  {builder}");

            builder.Clear();
        }
    }
}