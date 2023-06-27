using System;

namespace Prelude;

public abstract class Result<T, TErr> : IEquatable<Result<T, TErr>>
{
    public static Result<T, TErr> Of(T value)
    {
        return new Ok<T, TErr>(value);
    }

    public static Result<T, TErr> Err(TErr value)
    {
        return new Err<T, TErr>(value);
    }
        
    public abstract bool IsOk();
    public abstract bool IsErr();
    public abstract Option<T> OkValue();
    public abstract Option<TErr> ErrValue();
    public abstract bool IsOkAnd(Predicate<T> predicate);
    public abstract bool IsErrAnd(Predicate<TErr> predicate);
    public abstract Result<T2, TErr> SelectOk<T2>(Func<T, T2> func);
    public abstract Result<T, E2> SelectErr<E2>(Func<TErr, E2> func);
    public abstract Result<T2, TErr> SelectMany<T2>(Func<T, Result<T2, TErr>> func);
    public abstract bool Equals(Result<T, TErr>? other);
}

internal class Ok<T, TErr> : Result<T, TErr>
{
    private readonly T _value;

    protected internal Ok(T value)
    {
        _value = value;
    }

    public override bool IsOk()
    {
        return true;
    }

    public override bool IsErr()
    {
        return false;
    }

    public override Option<T> OkValue()
    {
        return Option<T>.Some(_value);
    }

    public override Option<TErr> ErrValue()
    {
        return Option<TErr>.None();
    }

    public override bool IsOkAnd(Predicate<T> predicate)
    {
        return predicate(_value);
    }

    public override bool IsErrAnd(Predicate<TErr> predicate)
    {
        return false;
    }

    public override Result<T2, TErr> SelectOk<T2>(Func<T, T2> func)
    {
        return new Ok<T2, TErr>(func(_value));
    }

    public override Result<T, E2> SelectErr<E2>(Func<TErr, E2> func)
    {
        return new Ok<T, E2>(_value);
    }

    public override Result<T2, TErr> SelectMany<T2>(Func<T, Result<T2, TErr>> func)
    {
        return func(_value);
    }

    public override bool Equals(Result<T, TErr>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other.GetType() != this.GetType()) return false;
        return other.OkValue().Unwrap()!.Equals(OkValue().Unwrap());
    }
}

internal class Err<T, TErr> : Result<T, TErr>
{
    private readonly TErr _value;
        
    protected internal Err(TErr value)
    {
        _value = value;
    }
        
    public override bool IsOk()
    {
        return false;
    }

    public override bool IsErr()
    {
        return true;
    }

    public override Option<T> OkValue()
    {
        return Option<T>.None();
    }

    public override Option<TErr> ErrValue()
    {
        return Option<TErr>.Some(_value);
    }

    public override bool IsOkAnd(Predicate<T> predicate)
    {
        return false;
    }

    public override bool IsErrAnd(Predicate<TErr> predicate)
    {
        return predicate(_value);
    }

    public override Result<TOut, TErr> SelectOk<TOut>(Func<T, TOut> func)
    {
        return new Err<TOut, TErr>(_value);
    }

    public override Result<T, TErrOut> SelectErr<TErrOut>(Func<TErr, TErrOut> func)
    {
        return new Err<T, TErrOut>(func(_value));
    }

    public override Result<TOut, TErr> SelectMany<TOut>(Func<T, Result<TOut, TErr>> func)
    {
        return new Err<TOut, TErr>(_value);
    }


    public override bool Equals(Result<T, TErr>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other.GetType() != this.GetType()) return false;
        return other.ErrValue().Unwrap()!.Equals(ErrValue().Unwrap());
    }
}