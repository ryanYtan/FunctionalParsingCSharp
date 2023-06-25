namespace SPA.Utility;

public interface IBiMap<TLeft, TRight>
{
    IEnumerable<KeyValuePair<TLeft, TRight>> EnumerateLeftToRight();
    IEnumerable<KeyValuePair<TRight, TLeft>> EnumerateRightToLeft();
    TRight GetLeftToRight(TLeft key);
    bool TryGetLeftToRight(TLeft key, out TRight value);
    TLeft GetRightToLeft(TRight key);
    bool TryGetRightToLeft(TRight key, out TLeft value);
    bool ContainsLeft(TLeft key);
    bool ContainsRight(TRight key);
    KeyValuePair<TLeft, TRight> GetFromLeft(TLeft key);
    KeyValuePair<TLeft, TRight> GetFromRight(TRight key);
    int Count { get; }    
}

public class BiMap<TLeft, TRight> : IBiMap<TLeft, TRight>
{
    private readonly Dictionary<TLeft, TRight> _leftToRight;
    private readonly Dictionary<TRight, TLeft> _rightToLeft;
    
    public BiMap(IEqualityComparer<TLeft> leftComp = null, IEqualityComparer<TRight> rightComp = null)
    {
        _leftToRight = new Dictionary<TLeft, TRight>(leftComp);
        _rightToLeft = new Dictionary<TRight, TLeft>(rightComp);
    }

    public bool AddPair(TLeft left, TRight right)
    {
        if (ContainsLeft(left) || ContainsRight(right))
        {
            return false;
        }
        _leftToRight.Add(left, right);
        _rightToLeft.Add(right, left);
        return true;
    }
    
    public IEnumerable<KeyValuePair<TLeft, TRight>> EnumerateLeftToRight()
    {
        return _leftToRight;
    }

    public IEnumerable<KeyValuePair<TRight, TLeft>> EnumerateRightToLeft()
    {
        return _rightToLeft;
    }

    public TRight GetLeftToRight(TLeft key)
    {
        if (!TryGetLeftToRight(key, out var value))
        {
            throw new KeyNotFoundException($"Key '{key}' not found in left-to-right mapping");
        }
        return value;
    }

    public bool TryGetLeftToRight(TLeft key, out TRight value)
    {
        return _leftToRight.TryGetValue(key, out value);
    }

    public TLeft GetRightToLeft(TRight key)
    {
        if (!TryGetRightToLeft(key, out var value))
        {
            throw new KeyNotFoundException($"Key '{key}' not found in right-to-left mapping");
        }
        return value;
    }

    public bool TryGetRightToLeft(TRight key, out TLeft value)
    {
        return _rightToLeft.TryGetValue(key, out value);
    }

    public bool ContainsLeft(TLeft key)
    {
        return TryGetLeftToRight(key, out _);
    }

    public bool ContainsRight(TRight key)
    {
        return TryGetRightToLeft(key, out _);
    }

    public KeyValuePair<TLeft, TRight> GetFromLeft(TLeft key)
    {
        TRight right = GetLeftToRight(key);
        return new KeyValuePair<TLeft, TRight>(key, right);
    }

    public KeyValuePair<TLeft, TRight> GetFromRight(TRight key)
    {
        TLeft left = GetRightToLeft(key);
        return new KeyValuePair<TLeft, TRight>(left, key);
    }

    public int Count => _leftToRight.Count;
}