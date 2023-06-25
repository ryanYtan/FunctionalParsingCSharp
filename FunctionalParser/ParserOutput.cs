using Prelude;

namespace FunctionalParser;
    public class ParserOutput<T, R>
    {
        public Option<T> Value { get; }
        public R Remaining { get; }
        public bool IsSuccess { get; }

        private ParserOutput(Option<T> value, R remaining, bool isSuccess)
        {
            Value = value;
            Remaining = remaining;
            IsSuccess = isSuccess;
        }

        public static ParserOutput<T, R> Success(T value, R remaining)
        {
            return new ParserOutput<T, R>(Option<T>.Some(value), remaining, true);
        }
        
        public static ParserOutput<T, R> SuccessEmpty(R remaining)
        {
            return new ParserOutput<T, R>(Option<T>.None(), remaining, true);
        }

        public static ParserOutput<T, R> Fail(R remaining)
        {
            return new ParserOutput<T, R>(Option<T>.None(), remaining, false);
        }

        public void Deconstruct(out Option<T> value, out R remaining, out bool isSuccess)
        {
            value = Value;
            remaining = Remaining;
            isSuccess = IsSuccess;
        }
    }
