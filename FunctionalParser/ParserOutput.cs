using Prelude;

namespace FunctionalParser;
    public class ParserOutput<TValue, TStream>
    {
        public Option<TValue> Value { get; }
        public TStream Remaining { get; }
        public bool IsSuccess { get; }

        private ParserOutput(Option<TValue> value, TStream remaining, bool isSuccess)
        {
            Value = value;
            Remaining = remaining;
            IsSuccess = isSuccess;
        }

        public static ParserOutput<TValue, TStream> Success(TValue value, TStream remaining)
        {
            return new ParserOutput<TValue, TStream>(Option<TValue>.Some(value), remaining, true);
        }
        
        public static ParserOutput<TValue, TStream> SuccessEmpty(TStream remaining)
        {
            return new ParserOutput<TValue, TStream>(Option<TValue>.None(), remaining, true);
        }

        public static ParserOutput<TValue, TStream> Fail(TStream remaining)
        {
            return new ParserOutput<TValue, TStream>(Option<TValue>.None(), remaining, false);
        }

        public void Deconstruct(out Option<TValue> value, out TStream remaining, out bool isSuccess)
        {
            value = Value;
            remaining = Remaining;
            isSuccess = IsSuccess;
        }
    }
