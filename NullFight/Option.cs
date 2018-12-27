using System;
using System.Diagnostics;

namespace NullFight
{
    [DebuggerDisplay("{!HasValue ? \"Some('\"+Value.ToString()+\"')\" : \"None\", nq}")]
    public partial struct Option<T>
    {
        /// <summary>
        /// Creates a new option object. Options should be constructed using the Extension methods.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="hasValue">Is the value present</param>
        internal Option(T value, bool hasValue)
        {
            Value = value;
            HasValue = hasValue;
        }


        public T Value { get; }

        public bool HasValue { get; }

        /// <summary>
        /// Returns the Value unless this option is a None. In that case it returns the passed in value.
        /// </summary>
        /// <param name="valueIfNone">Value to return if the option is a None</param>
        /// <returns>Value or Default Passed Value</returns>
        public T ValueOr(T valueIfNone)
        {
            return HasValue ? Value : valueIfNone;
        }
      
        /// <summary>
        /// Returns the value if present or throws an error
        /// </summary>
        /// <param name="ex">Exception to throw if value is not present</param>
        /// <returns>Value</returns>
        public T GetValueOrThrow(Exception ex)
        {
            if (HasValue)
                return Value;
            throw ex;
        }

        /// <summary>
        /// Returns the value if present or throws an error
        /// </summary>
        /// <returns>Value</returns>
        public T GetValueOrThrow(string exceptionMessage = null)
        {
            return GetValueOrThrow(new Exception(exceptionMessage ?? "Value not present"));
        }

        // Used to cast a result to a Result<object>
        public static implicit operator Option<object>(Option<T> myOption)
        {
            return myOption.MapValue(x => x as object);
        }

        // Used so that ResultError, which creates a Result<object> if you provide no type arguments, can be cast
        // to any other Result of type since the Value does not matter in that case.
        public static implicit operator Option<T>(Option<object> myOption)
        {
            return myOption.MapValue(x => default(T));
        }

        public Option<TK> MapValue<TK>(Func<T, TK> valueMapFunc)
        {
            if (HasValue)
                return new Option<TK>(valueMapFunc(Value), true);
            else
                return new Option<TK>(default(TK), false);
        }

        public TK Match<TK>(Func<T, TK> onValueFunc, Func<TK> onNone)
        {
            return HasValue ? onValueFunc(Value) : onNone();
        }

        public void Match(Action<T> onValue, Action onNone)
        {
            if (HasValue)
            {
                onValue(Value);
            }
            else
            {
                onNone();
            }
        }
    }
}