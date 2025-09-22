using System.Text;
using BallSort.Engine.Game;
using BallSort.Engine.Infrastructure;
using BallSort.Engine.Models;
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
            
            _testOutputHelper.WriteLine(string.Empty);
        
            _testOutputHelper.WriteLine($"  Steps: {result.Moves.Count}.");
        }
    }
    
    [Fact]
    public void SolveReturnsFalseForUnsolvable()
    {
        var board = new Board(new Puzzle
        {
            Data = new Data
            {
                Layout = [1, 2, 3, 4, 4, 3, 2, 1, 2, 1, 3, 4, 0, 0, 0, 0]
            },
            GridWidth = 4,
            GridHeight = 4
        });
        
        var solver = new Solver(board);

        var result = solver.Solve();

        Assert.False(result.Solved);
    }

    private void DumpBoard(Board board)
    {
        _testOutputHelper.WriteLine(string.Empty);
        
        var builder = new StringBuilder();

        var columns = new List<Colour[]>();
        
        for (var x = 0; x < board.Width; x++)
        {
            columns.Add(board.GetColumn(x).ToArray());
        }

        for (var y = 0; y < board.Height; y++)
        {
            for (var x = 0; x < board.Width; x++)
            {
                var ball = columns[x][y];

                if (ball == Colour.Empty)
                {
                    builder.Append('-');
                }
                else
                {
                    builder.Append((char) ('@' + (int) columns[x][y]));
                }
            }

            _testOutputHelper.WriteLine($"  {builder}");

            builder.Clear();
        }
    }
}