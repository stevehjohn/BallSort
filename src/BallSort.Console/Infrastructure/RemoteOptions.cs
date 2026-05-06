using BallSort.Engine.Infrastructure;
using CommandLine;
using JetBrains.Annotations;

namespace BallSort.Console.Infrastructure;

[UsedImplicitly]
[Verb("remote", HelpText = "Run puzzles from Puzzle Madness.")]
public class RemoteOptions
{
    [Option('d', "difficulty", Required = true, HelpText = "The class of puzzles to solve.")]
    public Difficulty Difficulty { get; [UsedImplicitly] set; }
    
    [Option('q', "quantity", Required = true, HelpText = "The number of puzzles to solve.")]
    public int Quantity { get; [UsedImplicitly] set; }
    
    [Option('y', "year", Required = false, HelpText = "The year of the puzzle.")]
    public int Year { get; [UsedImplicitly] set; }
    
    [Option('m', "month", Required = false, HelpText = "The month of the puzzle.")]
    public int Month { get; [UsedImplicitly] set; }
    
    [Option('a', "day", Required = false, HelpText = "The day of the puzzle.")]
    public int Day { get; [UsedImplicitly] set; }
}