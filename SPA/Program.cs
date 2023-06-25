using System.Text;
using SPA;

string ReadInputFromStdin()
{
    StringBuilder sb = new();
    int c;
    while ((c = Console.Read()) != -1)
    {
        sb.Append((char)c);
    }
    return sb.ToString();
}

var program = ReadInputFromStdin();

var (tokens, _, _) = ProgramLexer.FullProgram.Invoke(program);

tokens
    .OrElseThrow(() => new Exception("unable to lex program"))
    .Unwrap()
    .Where(x => !string.IsNullOrWhiteSpace(x))
    .Select(Token.Of)
    .ToList()
    .ForEach(Console.WriteLine);