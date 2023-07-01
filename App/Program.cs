using App;

void HandleJson()
{
}

void HandleSimple()
{
}

var commandLineArgs = CommandLineHandler.ParseCliArgs(args);

switch (commandLineArgs.WhichParser)
{
    case ParserType.Json:
        HandleJson();
        break;
    case ParserType.Simple:
        HandleSimple();
        break;
    default:
        throw new Exception("unreachable");
}
