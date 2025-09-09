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
        
        WriteLine($"  Solving puzzle #{options.PuzzleNumber}: {board.Width}x{board.Height}, {board.Colours} colours.");
        
        // TODO: Separate thread.
        var solver = new Solver(board);

        var stopwatch = Stopwatch.StartNew();

        var solution = solver.Solve();
        
        stopwatch.Stop();
        
        WriteLine();
        
        WriteLine($@"  Solved in {stopwatch.Elapsed:h\:mm\:ss\.fff}, steps: {solution.Count:N0}.");
        
        WriteLine();

        var i = 1;
        
        foreach (var step in solution)
        {
            WriteLine($"  Step {i:N0,3}: {step.Source:N0,2} -> {step.Target:N0,2}.");

            i++;
        }
        
        WriteLine();
    }
}