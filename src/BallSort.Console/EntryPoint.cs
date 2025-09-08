using BallSort.Console.Infrastructure;
using BallSort.Console.Runners;
using CommandLine;

namespace BallSort.Console;

public static class EntryPoint
{
    public static void Main(string[] arguments)
    {
        var parser = new Parser(settings => { settings.CaseInsensitiveEnumValues = true; });

        parser.ParseArguments<LocalOptions, RemoteOptions>(arguments)
            .WithParsed<LocalOptions>(options => new Local().Run(options))
            .WithParsed<RemoteOptions>(options => new Remote().Run(options));
    }
}