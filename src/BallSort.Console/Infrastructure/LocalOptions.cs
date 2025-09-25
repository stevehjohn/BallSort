// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Diagnostics.CodeAnalysis;
using CommandLine;
using JetBrains.Annotations;

namespace BallSort.Console.Infrastructure;

[ExcludeFromCodeCoverage]
[UsedImplicitly]
[Verb("local", HelpText = "Run a puzzle from the local file system.")]
public class LocalOptions
{
    [Option('n', "number", Required = true, HelpText = "The puzzle number to run.")]
    public int PuzzleNumber { get; set; }
}