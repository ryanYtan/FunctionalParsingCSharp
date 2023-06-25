using FunctionalParser;

namespace SPA;

using static LexerHelper;
using static Combinators;
using LexerOutput = ParserOutput<IEnumerable<string>, string>;
using Lexer = IParser<IEnumerable<string>, string>;

public static class LexerHelper
{
    // [ ["a", "b"], ["c"], ["de", "f"] ] => [ "abcdef" ]
    public static readonly Func<IEnumerable<IEnumerable<string>>, IEnumerable<string>> FullConcat =
        l => new[] { l.Select(x => x.Aggregate("", (y, z) => y + z)).Aggregate((a, b) => a + b) };
    
    // [ ["a", "b"], ["c"], ["de", "f"] ] => [ "ab", "c", "def" ]
    public static readonly Func<IEnumerable<IEnumerable<string>>, IEnumerable<string>> PartialConcat =
        l => l.Select(x => x.Aggregate((y, z) => y + z)).ToList();
    
    public static Lexer Char(char c)
    {
        return Range(c, c);
    }
    
    public static Lexer Range(char a, char b)
    {
        return s => string.IsNullOrEmpty(s) || s[0] < a || b < s[0]
            ? LexerOutput.Fail(s)
            : LexerOutput.Success(new List<string> { s[0].ToString() }, s[1..]);
    }
    
    public static Lexer CharSequence(string t)
    {
        return s => s.StartsWith(t)
            ? LexerOutput.Success(new List<string> { t }, s[t.Length..])
            : LexerOutput.Fail(s);
    }
    
    public static Lexer FullMatch(Lexer lexer)
    {
        return s =>
        {
            var (value, remaining, isSuccess) = lexer(s);
            return isSuccess && remaining.Length == 0
                ? LexerOutput.Success(value.Unwrap(), "")
                : LexerOutput.Fail(s);
        };
    }
}

public static class ProgramLexer
{
    private static readonly Lexer LetterLowercase = Range('a', 'z');
    private static readonly Lexer LetterUppercase = Range('A', 'Z');
    private static readonly Lexer Letter = Choice(LetterLowercase, LetterUppercase);
    private static readonly Lexer Digit = Range('0', '9');
    private static readonly Lexer Underscore = Char('_');
    private static readonly Lexer Whitespace = Choice(" \r\n\t".Select(Char).ToArray());

    public static Lexer SkipSpaces => Some(FullConcat, Whitespace);

    public static Lexer Identifier =>
        Sequence(
            FullConcat,
            Choice(Letter, Underscore),
            ZeroOrMore(FullConcat, Choice(Letter, Digit, Underscore))
        );

    public static Lexer Number
    {
        get
        {
            var sign = Choice(Char('+'), Char('-'));
            var integer = Some(FullConcat, Digit);
            var fraction = Sequence(
                FullConcat,
                Char('.'),
                Some(FullConcat, Digit)
            );
            var exponent = Sequence(
                FullConcat,
                Choice(Char('e'), Char('E')),
                ZeroOrOne(sign),
                Some(FullConcat, Digit)
            );
            var number = Sequence(
                FullConcat,
                ZeroOrOne(sign),
                integer,
                ZeroOrOne(fraction),
                ZeroOrOne(exponent),
                AssertNotAfter(Choice(Letter, Digit, Char('.')))
            );
            return number;
        }
    }

    public static Lexer LengthTwoStrings
    {
        get
        {
            return Choice(
                Token.TokenMap.EnumerateLeftToRight()
                    .Select(x => x.Value)
                    .Where(x => x.Length == 2)
                    .Select(CharSequence)
                    .ToArray()
            );
        }
    }

    public static Lexer LengthOneStrings
    {
        get
        {
            return Choice(
                Token.TokenMap.EnumerateLeftToRight()
                    .Select(x => x.Value)
                    .Where(x => x.Length == 1)
                    .Select(CharSequence)
                    .ToArray()
            );
        }
    }

    public static Lexer FullProgram =>
        FullMatch(ZeroOrMore(
            PartialConcat,
            Choice(
                SkipSpaces,
                Identifier,
                Number,
                LengthTwoStrings,
                LengthOneStrings
            )
        ));
}
