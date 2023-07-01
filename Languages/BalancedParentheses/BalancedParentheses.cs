using FunctionalParser;
using static FunctionalParser.Combinators;

namespace Languages.BalancedParentheses;

public class BalancedParentheses
{
    public static IParser<IEnumerable<string>, string> Char(char c)
    {
        return Range(c, c);
    }
    
    public static IParser<IEnumerable<string>, string> Range(char a, char b)
    {
        return s => string.IsNullOrEmpty(s) || s[0] < a || b < s[0]
            ? ParserOutput<IEnumerable<string>, string>.Fail(s)
            : ParserOutput<IEnumerable<string>, string>.Success(new List<string> { s[0].ToString() }, s[1..]);
    }
    
    static void f()
    {
        IParser<IEnumerable<string>, string>? expr = null;
        expr = Sequence(
            x => x.First(),
            Char('('),
            expr,
            Char(')')
        );
    }
}