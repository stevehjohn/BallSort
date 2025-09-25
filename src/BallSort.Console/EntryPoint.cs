using System.Diagnostics.CodeAnalysis;
using BallSort.Console.Infrastructure;
using BallSort.Console.Runners;
using CommandLine;

namespace BallSort.Console;

[ExcludeFromCodeCoverage]
public static class EntryPoint
{
    public static void Main(string[] arguments)
    {
        var parser = new Parser(settings => { settings.CaseInsensitiveEnumValues = true; });

        parser.ParseArguments<LocalOptions>(arguments)
            .WithParsed(Local.Run);
    }
}