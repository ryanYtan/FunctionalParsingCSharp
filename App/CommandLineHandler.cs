using Mono.Options;

namespace App;

public enum ParserType
{
    Simple,
    Json,
}

public class CommandLineArgs
{
    public int Verbosity { get; set; }
    public bool ShouldShowHelp { get; set; }
    public ParserType WhichParser { get; set; }
    public List<string> Extra { get; set; }
}

public static class CommandLineHandler
{
    public static CommandLineArgs ParseCliArgs(params string[] programArgs)
    {
        var args = new CommandLineArgs();
        var options = new OptionSet
        {
            {
                "p|parser=",
                "parser type",
                p =>
                {
                    var pType = p switch
                    {
                        "json" => ParserType.Json,
                        "simple" => ParserType.Simple,
                        _ => throw new OptionException("could not find that parser", "parser"),
                    };
                    args.WhichParser = pType;
                }
            },
            {
                "v",
                "increase debug message verbosity",
                v =>
                {
                    if (v != null)
                    {
                        args.Verbosity++;
                    }
                }
            },
            {
                "h|help",
                "show this message and exit",
                help =>
                {
                    args.ShouldShowHelp = help != null;
                }
            },
        };

        try
        {
            args.Extra = options.Parse(programArgs);
            return args;
        }
        catch (OptionException e)
        {
            Console.Error.WriteLine(e);
            throw;
        }
    }
}