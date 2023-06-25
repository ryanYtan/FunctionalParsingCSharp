using Prelude;

namespace SPA.Utility;
    
public class PeekableBuffer<T>
{
    private readonly IList<T> _iter;
    private int _cursor;

    private PeekableBuffer(IList<T> iter, int cursor)
    {
        _iter = iter;
        _cursor = cursor;
    }

    public static PeekableBuffer<T> Of(IEnumerable<T> iter)
    {
        return new PeekableBuffer<T>(iter.ToList(), 0);
    }

    public Option<T> LookAround(int offset)
    {
        var computedIndex = offset < 0
            ? Math.Max(0, (long)_cursor + offset)
            : Math.Min(_iter.Count, (long)_cursor + offset);
        return _cursor == _iter.Count
            ? Option<T>.Some(_iter[(int)computedIndex])
            : Option<T>.None();
    }

    public void Shift(int offset)
    {
        var computedIndex = offset < 0
            ? Math.Max(0, (long)_cursor + offset)
            : Math.Min(_iter.Count, (long)_cursor + offset);
        _cursor = (int)computedIndex;
    }

    public int Position()
    {
        return _cursor;
    }

    public Option<T> Current()
    {
        return LookAround(0);
    }

    public Option<T> Peek()
    {
        return LookAround(1);
    }

    public void Advance()
    {
        Shift(1);
    }

    public Option<T> Consume()
    {
        var curr = Current();
        Advance();
        return curr;
    }

    public PeekableBuffer<T> SliceFrom(int pos)
    {
        return Of(_iter.Skip(pos));
    }

    public PeekableBuffer<T> SliceBetween(int from, int to)
    {
        return Of(_iter.Skip(from).Take(to - from));
    }
}
