using FunctionalParser;

namespace SPA;

using static ParserHelper;
using static Combinators;
using ParserOutput = ParserOutput<Node, IList<Token>>;
using Parser = IParser<Node, IList<Token>>;
public delegate Node NodeBuilder(IList<Token> tokens);

public static class ParserHelper
{
    public static Parser Expect(TokenType type)
    {
        return toks => toks.Any() && toks[0].Is(type)
            ? ParserOutput.SuccessEmpty(toks.Skip(1).ToList())
            : ParserOutput.Fail(toks);
    }

    public static Parser Expect(string charSequence)
    {
        return toks => toks.Any() && toks[0].Is(charSequence)
            ? ParserOutput.SuccessEmpty(toks.Skip(1).ToList())
            : ParserOutput.Fail(toks);
    }

    public static Parser ExpectSeries(params string[] charSequences)
    {
        return toks =>
        {
            if (toks.Count < charSequences.Length)
            {
                return ParserOutput.Fail(toks);
            }
            var n = charSequences.Length;
            return toks.Take(n).Select((s, i) => s.Is(charSequences[i])).All(b => b)
                ? ParserOutput.SuccessEmpty(toks.Skip(n).ToList())
                : ParserOutput.Fail(toks);
        };
    }
}