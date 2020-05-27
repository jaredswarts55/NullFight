using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace NullFight
{
    /// <summary>
    /// A struct that models the fact that a null value may have been returned and forces
    /// the user of a method returning it to deal with that null value gracefully.
    /// </summary>
    [DebuggerDisplay("{HasValue ? \"Some('\"+Value.ToString()+\"')\" : \"None\", nq}")]
    [DataContract]
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


        /// <summary>
        /// The Options Value, which might be null, check HasValue before using.
        /// </summary>
        [DataMember]
        public T Value { get; private set; }

        /// <summary>
        /// If 'Value' is not null returns true
        /// </summary>
        [DataMember]
        public bool HasValue { get; private set; }

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

        /// <summary>
        /// Used to cast a result to a Result&lt;object&gt;
        /// </summary>
        public static implicit operator Option<object>(Option<T> myOption)
        {
            return myOption.MapValue(x => x as object);
        }

        /// <summary>
        /// Used so that ResultError, which creates a Result&lt;object&gt; if you provide no type arguments, can be cast
        /// to any other Result of type since the Value does not matter in that case.
        /// </summary>
        public static implicit operator Option<T>(Option<object> myOption)
        {
            return myOption.MapValue(x => default(T));
        }

        /// <summary>
        /// Maps a value if there is one to a different value using the provided function. This method
        /// is not called if the Option is None
        /// </summary>
        /// <param name="valueMapFunc">Function to run if this option has a value</param>
        /// <returns>Value returned from the passed in method</returns>
        public Option<TK> MapValue<TK>(Func<T, TK> valueMapFunc)
        {
            if (HasValue)
                return new Option<TK>(valueMapFunc(Value), true);
            else
                return new Option<TK>(default(TK), false);
        }

        /// <summary>
        /// Runs one of two functions that return a value on the option's value. Runs 'On Value' if the
        /// Option has a value and runs 'On None' if it doesn't. The return value from one of these functions
        /// is returned.
        /// </summary>
        /// <param name="onValue">Function to run if there is a value</param>
        /// <param name="onNone">Function to run if there is no value</param>
        /// <returns>The value returned from one of the functions</returns>
        public TK Match<TK>(Func<T, TK> onValue, Func<TK> onNone)
        {
            return HasValue ? onValue(Value) : onNone();
        }

        /// <summary>
        /// Runs one of two functions on the option's value. Runs 'On Value' if the
        /// Option has a value and runs 'On None' if it doesn't. 
        /// </summary>
        /// <param name="onValue">Function to run if there is a value</param>
        /// <param name="onNone">Function to run if there is no value</param>
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