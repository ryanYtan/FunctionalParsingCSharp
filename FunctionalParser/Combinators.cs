using Prelude;

namespace FunctionalParser;

public delegate ParserOutput<T, R> IParser<T, R>(R stream);

public static class Combinators
{
    public static IParser<T, R> Recursive<T, R>(Func<IParser<T, R>> parser)
    {
        return r =>
        {
            var func = new Lazy<IParser<T, R>>(parser).Value;
            return func(r);
        };
    }

    public static IParser<T, R> Nothing<T, R>()
    {
        return ParserOutput<T, R>.SuccessEmpty;
    }

    public static IParser<T, R> Choice<T, R>(params IParser<T, R>[] parsers)
    {
        return r => parsers
            .Select(p => p(r))
            .FirstOrDefault(output => output.IsSuccess, ParserOutput<T, R>.Fail(r));
    }

    public static IParser<T, R> Some<T, R>(Func<IEnumerable<T>, T> reducer, IParser<T, R> parser)
    {
        return r =>
        {
            var results = InfList.Iterate(parser(r), output => parser(output.Remaining))
                .TakeWhile(output => output.IsSuccess)
                .ToList();
            if (results.Any())
            {
                var elems = results
                    .Select(result => result.Value.Unwrap());
                return ParserOutput<T, R>.Success(reducer(elems), results.Last().Remaining);
            }

            return ParserOutput<T, R>.Fail(r);
        };
    }

    public static IParser<T, R> Sequence<T, R>(Func<IEnumerable<T>, T> reducer, params IParser<T, R>[] parsers)
    {
        return r =>
        {
            var (_, remaining, _) = ParserOutput<T, R>.Fail(r); //placeholder
            var builder = new List<T>();
            foreach (var parser in parsers)
            {
                (var value, remaining, var isSuccess) = parser(remaining);
                if (!isSuccess)
                {
                    return ParserOutput<T, R>.Fail(r);
                }

                if (value.IsSome())
                {
                    builder.Add(value.Unwrap());
                }
            }

            return ParserOutput<T, R>.Success(reducer(builder), remaining);
        };
    }

    public static IParser<T, R> ZeroOrMore<T, R>(Func<IEnumerable<T>, T> reducer, IParser<T, R> parser)
    {
        return Choice(Some(reducer, parser), Nothing<T, R>());
    }

    public static IParser<T, R> ZeroOrOne<T, R>(IParser<T, R> parser)
    {
        return Choice(parser, Nothing<T, R>());
    }

    public static IParser<T, R> AssertAfter<T, R>(IParser<T, R> parser)
    {
        return r => parser(r).IsSuccess
            ? ParserOutput<T, R>.SuccessEmpty(r)
            : ParserOutput<T, R>.Fail(r);
    }

    public static IParser<T, R> AssertNotAfter<T, R>(IParser<T, R> parser)
    {
        return r => parser(r).IsSuccess
            ? ParserOutput<T, R>.Fail(r)
            : ParserOutput<T, R>.SuccessEmpty(r);
    }
}
