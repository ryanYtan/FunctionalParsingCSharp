using System;

namespace Prelude
{
    public class NoElementExistsException : Exception
    {
        public NoElementExistsException()
        {
        }

        public NoElementExistsException(string message) : base(message)
        {
        }

        public NoElementExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public abstract class Option<T>
    {
        public static Option<T> Some(T value)
        {
            return new Some<T>(value);
        }

        public static Option<T> None()
        {
            return new None<T>();
        }
        
        public abstract Option<TOut> Select<TOut>(Func<T, TOut> func);
        public abstract Option<TOut> SelectMany<TOut>(Func<T, Option<TOut>> func);
        public abstract Option<T> Where(Predicate<T> predicate);
        public abstract Option<T> OrElse(Func<Option<T>> generator);
        public abstract Option<T> OrElseThrow(Func<Exception> exceptionThrower);
        public abstract bool IsSome();
        public abstract bool IsNone();
        public abstract T Unwrap();
    }

    internal class Some<T> : Option<T>
    {
        private readonly T _data;

        protected internal Some(T value)
        {
            _data = value;
        }

        public override Option<TOut> Select<TOut>(Func<T, TOut> func)
        {
            return new Some<TOut>(func(_data));
        }

        public override Option<TOut> SelectMany<TOut>(Func<T, Option<TOut>> func)
        {
            return func(_data);
        }

        public override Option<T> Where(Predicate<T> predicate)
        {
            if (predicate(_data))
            {
                return this;
            }
            return new None<T>();
        }

        public override Option<T> OrElse(Func<Option<T>> generator)
        {
            return this;
        }

        public override Option<T> OrElseThrow(Func<Exception> exceptionThrower)
        {
            return this;
        }

        public override bool IsSome()
        {
            return true;
        }

        public override bool IsNone()
        {
            return false;
        }

        public override T Unwrap()
        {
            return _data;
        }
    }

    internal class None<T> : Option<T>
    {
        protected internal None()
        {
        }

        public override Option<TOut> Select<TOut>(Func<T, TOut> func)
        {
            return new None<TOut>();
        }

        public override Option<TOut> SelectMany<TOut>(Func<T, Option<TOut>> func)
        {
            return new None<TOut>();
        }

        public override Option<T> Where(Predicate<T> predicate)
        {
            return new None<T>();
        }

        public override Option<T> OrElse(Func<Option<T>> generator)
        {
            return generator();
        }

        public override Option<T> OrElseThrow(Func<Exception> exceptionThrower)
        {
            throw exceptionThrower();
        }

        public override bool IsSome()
        {
            return false;
        }

        public override bool IsNone()
        {
            return true;
        }

        public override T Unwrap()
        {
            throw new NoElementExistsException("No element exists in this Option");
        }
    }
}