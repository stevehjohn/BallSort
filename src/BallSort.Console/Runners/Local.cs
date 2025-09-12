using System.Diagnostics;
using BallSort.Console.Infrastructure;
using BallSort.Engine;
using BallSort.Engine.Infrastructure;
using static System.Console;

namespace BallSort.Console.Runners;

public class Local
{
    public void Run(LocalOptions options)
    {
        PuzzleManager.Path = "Data/Puzzles.json";
        
        var board = PuzzleManager.Instance.GetPuzzle(options.PuzzleNumber);
        
        Clear();
        
        WriteLine();
        
        WriteLine($"  Solving puzzle number {options.PuzzleNumber}: {board.Width}x{board.Height}, {board.Colours} colours.");
        
        // TODO: Separate thread.
        var solver = new Solver(board);

        var stopwatch = Stopwatch.StartNew();

        var solution = solver.Solve();

        var moves = solution.Moves;
        
        stopwatch.Stop();
        
        WriteLine();
        
        WriteLine($@"  Solved in {stopwatch.Elapsed:h\:mm\:ss\.fff}, steps: {moves.Count:N0}.");
        
        WriteLine();

        var i = 1;
        
        foreach (var step in moves)
        {
            WriteLine($"  Step {i,3:N0}: {step.Source,2:N0} -> {step.Target,2:N0}.");

            i++;
        }
        
        WriteLine();
    }
}