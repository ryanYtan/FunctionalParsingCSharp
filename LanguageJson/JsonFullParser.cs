using FunctionalParser;

namespace LanguageJson;

using static JsonHelper;
using static Combinators;
using JsonParserOutput = ParserOutput<IEnumerable<string>, string>;
using JsonParser = IParser<IEnumerable<string>, string>;

public static class JsonHelper
{
    // [ ["a", "b"], ["c"], ["de", "f"] ] => [ "abcdef" ]
    public static readonly Func<IEnumerable<IEnumerable<string>>, IEnumerable<string>> FullConcat =
        l => new[] { l.Select(x => x.Aggregate("", (y, z) => y + z)).Aggregate((a, b) => a + b) };
    
    // [ ["a", "b"], ["c"], ["de", "f"] ] => [ "ab", "c", "def" ]
    public static readonly Func<IEnumerable<IEnumerable<string>>, IEnumerable<string>> PartialConcat =
        l => l.Select(x => x.Aggregate((y, z) => y + z)).ToList();
    
    public static JsonParser Char(char c)
    {
        return Range(c, c);
    }
    
    public static JsonParser Range(char a, char b)
    {
        return s => string.IsNullOrEmpty(s) || s[0] < a || b < s[0]
            ? JsonParserOutput.Fail(s)
            : JsonParserOutput.Success(new List<string> { s[0].ToString() }, s[1..]);
    }
    
    public static JsonParser CharSequence(string t)
    {
        return s => s.StartsWith(t)
            ? JsonParserOutput.Success(new List<string> { t }, s[t.Length..])
            : JsonParserOutput.Fail(s);
    }

    public static JsonParser FullMatch(JsonParser lexer)
    {
        return s =>
        {
            var (value, remaining, isSuccess) = lexer(s);
            return isSuccess && remaining.Length == 0
                ? JsonParserOutput.Success(value.Unwrap(), "")
                : JsonParserOutput.Fail(s);
        };
    }
}

public class JsonFullParser
{
    public static JsonParser DigitNine => Range('1', '9');
    public static JsonParser Digit => Range('0', '9');
    public static JsonParser HexDigit => Choice(Digit, Range('A', 'F'), Range('a', 'f'));
    public static JsonParser AsciiLowercase => Range('a', 'z');
    public static JsonParser AsciiUppercase => Range('A', 'Z');
    public static JsonParser AsciiLetter => Choice(AsciiLowercase, AsciiUppercase);
    public static JsonParser AsciiAlphaNum => Choice(AsciiLetter, Digit);

    //Number = '-'?('0'|{Digit9}{Digit}*)('.'{Digit}+)?([Ee][+-]?{Digit}+)?
    public static JsonParser Number => Some(FullConcat, Digit); //INCOMPLETE
    
    //String = '"'({Unescaped}|'\'(["\/bfnrt]|'u'{Hex}{Hex}{Hex}{Hex}))*'"'
    public static JsonParser Str => Sequence(FullConcat, Char('"'), AsciiAlphaNum, Char('"')); //INCOMPLETE
    
    public static JsonParser Object =>
        Choice(Char('{'), Char('}'), Sequence(PartialConcat, Char('{'), Members, Char('}')));

    public static JsonParser Members => Choice(Pair, Sequence(PartialConcat, Pair, Char(','), Recursive(() => Members)));
    
    public static JsonParser Pair => Sequence(PartialConcat, Str, Char(':'), Value);
    
    public static JsonParser Array => 
        Choice(Char('{'), Char('}'), Sequence(PartialConcat, Char('{'), Elements, Char('}')));

    public static JsonParser Elements => Choice(Value, Sequence(PartialConcat, Value, Char(','), Recursive(() => Elements)));

    public static JsonParser Value => Choice(
        Str,
        Number,
        Object,
        Array,
        CharSequence("true"),
        CharSequence("false"),
        CharSequence("null")
    );


    public static JsonParser Json => Choice(Object, Array);
}