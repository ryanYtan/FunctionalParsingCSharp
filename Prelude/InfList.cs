using System;
using System.Collections.Generic;
using System.Linq;

namespace Prelude;

public static class InfList
{
    public static IEnumerable<T> Cycle<T>(this IEnumerable<T> instance)
    {
        IList<T> lst = instance.ToList();
        if (lst.Count == 0)
        {
            throw new ArgumentException("Attempting to cycle on empty IEnumerable");
        }
        var i = 0;
        while (true)
        {
            i = i >= lst.Count ? i % lst.Count : i;
            yield return lst[i];
            i++;
        }
    }

    public static IEnumerable<T> Replicate<T>(this T item, uint count = uint.MaxValue)
    {
        for (var i = 0; i < count; i++)
        {
            yield return item;
        }
    }

    public static IEnumerable<T> Iterate<T>(this T seed, Func<T, T> next)
    {
        while (true)
        {
            yield return seed;
            seed = next(seed);
        }
    }

    public static IEnumerable<T> Generate<T>(Func<T> supplier)
    {
        while (true)
        {
            yield return supplier();
        }
    }
}