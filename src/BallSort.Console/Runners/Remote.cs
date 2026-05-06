using System.Diagnostics;
using BallSort.Console.Infrastructure;
using BallSort.Engine;
using BallSort.Engine.Extensions;
using BallSort.Engine.Game;
using BallSort.Engine.Infrastructure;
using BallSort.Engine.Models;
using static System.Console;

namespace BallSort.Console.Runners;

public class Remote
{
    private int _top;

    private int _count;

    private readonly Stopwatch _stopwatch = new();

    public void Run(RemoteOptions options)
    {
        var client = new PuzzleClient();
        
        Clear();

        var startTime = DateTime.Now;

        for (var i = 0; i < options.Quantity; i++)
        {
            WriteLine();

            WriteLine($"Fetching {options.Difficulty.ToString().ToLowerInvariant()} puzzle {i + 1:N0} of {options.Quantity:N0}...");

            WriteLine();

            (DateOnly Date, Puzzle Puzzle)? puzzle = null;

            for (var retry = 1; retry < 21; retry++)
            {
                try
                {
                    if (options.Year > 0)
                    {
                        puzzle = client.GetPuzzle(options.Difficulty, new DateOnly(options.Year, options.Month, options.Day));
                    }
                    else
                    {
                        puzzle = client.GetNextPuzzle(options.Difficulty);
                    }
                }
                catch
                {
                    //
                }

                if (puzzle != null)
                {
                    break;
                }

                var sleep = (int) Math.Pow(retry, 2);

                for (var timer = 0; timer < sleep; timer++)
                {
                    if (retry > 1)
                    {
                        CursorTop -= 2;
                    }

                    WriteLine($"Waiting for {sleep - timer:N0}s before attempt {retry}.  ");

                    WriteLine();

                    Thread.Sleep(1_000);

                    CursorTop -= 2;
                    
                    WriteLine("Retrying...                         ");
                    
                    WriteLine();
                }
            }

            Clear();

            if (puzzle == null)
            {
                WriteLine("No puzzles available.");

                break;
            }

            WriteLine();
            
            WriteLine($@"Started: {startTime:F}, runtime: {DateTime.Now - startTime:h\:mm\:ss\.fff}.");

            WriteLine();
            
            WriteLine($"Solving {options.Difficulty.ToString().ToLowerInvariant()} puzzle for {puzzle.Value.Date:R} ({puzzle.Value.Puzzle.GridWidth}x{puzzle.Value.Puzzle.GridHeight}). {i + 1:N0} / {options.Quantity:N0}.");

            WriteLine();
            
            var board = new Board(puzzle.Value.Puzzle);

            board.Dump();

            WriteLine();

            _top = CursorTop;

            _count = 0;

            _stopwatch.Restart();

            CursorVisible = false;

            var solver = new Solver(board);

            var result = solver.Solve();

            CursorVisible = true;

            _stopwatch.Stop();

            if (! result.Solved)
            {
                WriteLine("Unable to solve the puzzle.");

                WriteLine();

                break;
            }

            CursorTop = _top;

            foreach (var move in result.Moves)
            {
                WriteLine($"  {move.Source + 1,2} => {move.Target + 1,2}");
            }
            
            WriteLine();

            board.Dump();
            
            WriteLine();

            WriteLine(@$"Solved in {_stopwatch.Elapsed:h\:mm\:ss\.fff}, with {_count:N0} iterations.");
        }

        WriteLine();
    }
}