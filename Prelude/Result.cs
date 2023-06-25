using System;

namespace Prelude
{
    public abstract class Result<T, E>
    {
        public static Result<T, E> Of(T value)
        {
            return new Ok<T, E>(value);
        }

        public static Result<T, E> Err(E value)
        {
            return new Err<T, E>(value);
        }
        
        public abstract bool IsOk();
        public abstract bool IsErr();
        public abstract Option<T> OkValue();
        public abstract Option<E> ErrValue();
        public abstract bool IsOkAnd(Predicate<T> predicate);
        public abstract bool IsErrAnd(Predicate<E> predicate);
        public abstract Result<T2, E> SelectOk<T2>(Func<T, T2> func);
        public abstract Result<T, E2> SelectErr<E2>(Func<E, E2> func);
        public abstract Result<T2, E> SelectMany<T2>(Func<T, Result<T2, E>> func);
    }

    internal class Ok<T, E> : Result<T, E>
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

        public override Option<E> ErrValue()
        {
            return Option<E>.None();
        }

        public override bool IsOkAnd(Predicate<T> predicate)
        {
            return predicate(_value);
        }

        public override bool IsErrAnd(Predicate<E> predicate)
        {
            return false;
        }

        public override Result<T2, E> SelectOk<T2>(Func<T, T2> func)
        {
            return new Ok<T2, E>(func(_value));
        }

        public override Result<T, E2> SelectErr<E2>(Func<E, E2> func)
        {
            return new Ok<T, E2>(_value);
        }

        public override Result<T2, E> SelectMany<T2>(Func<T, Result<T2, E>> func)
        {
            return func(_value);
        }
    }

    internal class Err<T, E> : Result<T, E>
    {
        private readonly E _value;
        
        protected internal Err(E value)
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

        public override Option<E> ErrValue()
        {
            return Option<E>.Some(_value);
        }

        public override bool IsOkAnd(Predicate<T> predicate)
        {
            return false;
        }

        public override bool IsErrAnd(Predicate<E> predicate)
        {
            return predicate(_value);
        }

        public override Result<T2, E> SelectOk<T2>(Func<T, T2> func)
        {
            return new Err<T2, E>(_value);
        }

        public override Result<T, E2> SelectErr<E2>(Func<E, E2> func)
        {
            return new Err<T, E2>(func(_value));
        }

        public override Result<T2, E> SelectMany<T2>(Func<T, Result<T2, E>> func)
        {
            return new Err<T2, E>(_value);
        }
    }
}