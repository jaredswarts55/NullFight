using System;
using System.Diagnostics;
using NullFight.Exceptions;

namespace NullFight
{
    [DebuggerDisplay("{HasValue ? \"Some('\"+Value.ToString()+\"')\" : \"Error('\"+Exception.Message+\"')\", nq}")]
    public struct Result<K>
    {
        /// <summary>
        /// Creates a new Result object. Results should be constructed using the Extension methods.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exception"></param>
        internal Result(K value, Exception exception)
        {
            HasValue = exception == null;
            Value = value;
            Exception = exception;
        }

        private K Value { get; }
        private Exception Exception { get; }

        /// <summary>
        /// Checks if the Result has a value
        /// </summary>
        public bool HasValue { get; internal set; }


        // Used to cast a result to a Result<object>
        public static implicit operator Result<object>(Result<K> myResult)
        {
            return myResult.MapValue(x => x as object);
        }

        // Used so that ResultError, which creates a Result<object> if you provide no type arguments, can be cast
        // to any other Result of type since the Value does not matter in that case.
        public static implicit operator Result<K>(Result<object> myResult)
        {
            return myResult.MapValue(x => default(K));
        }

        /// <summary>
        /// An exception can be implicitly cast to a result error
        /// </summary>
        /// <param name="ex"></param>
        public static implicit operator Result<K>(Exception ex)
        {
            return new Result<K>(default(K), ex);
        }

        /// <summary>
        /// Get the value from the result. Only use if the value has been checked for before. MAKE SURE YOU HAVE CHECKED HasValue.
        /// </summary>
        /// <returns>Value if present</returns>
        public K Unwrap()
        {
            if (!HasValue)
                throw new ResultException("Unwrap failed because no value was present. Check for the value before using unwrap");
            return Value;
        }

        /// <summary>
        /// Gets the Exception or throws a generic exception if there was a value. MAKE SURE YOU HAVE CHECKED HasValue.
        /// </summary>
        /// <returns></returns>
        public Exception UnwrapException()
        {
            if (HasValue)
                throw new ResultException("UnwrapException failed because no Exception was present. Check for the value before using UnwrapException");
            return Exception;
        }

        public bool HasErrorWithType<TL>() where TL : Exception
        {
            if (HasValue)
                return false;
            if (Exception is TL)
                return true;
            return false;
        }

        /// <summary>
        /// Return the Exception as a more explicit type
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <returns></returns>
        public TK UnwrapException<TK>() where TK : Exception
        {
            return UnwrapException() as TK;
        }

        /// <summary>
        /// Return either the Exception or the Value as an object. For use with C#7 pattern matching
        /// </summary>
        /// <returns>Object of Exception or Value</returns>
        public object GetValueOrError()
        {
            return !HasValue ? (object)Exception : Value;
        }

        /// <summary>
        /// Used to bubble up a particular message if the value is not present.
        /// </summary>
        /// <param name="message">Error message if the value is not present</param>
        /// <returns>Value if present</returns>
        public K Expect(string message)
        {
            if (!HasValue)
                throw new FriendlyResultException(message, Exception);
            return Value;
        }

        /// <summary>
        /// Handle both the value and the exception with a delegate. Returning the same type.
        /// </summary>
        /// <typeparam name="TK">Type to return</typeparam>
        /// <param name="onValueFunc">Delegate for the Value</param>
        /// <param name="onErrorFunc">Delegate for the Error</param>
        /// <returns>A new mapped value</returns>
        public TK Match<TK>(Func<K, TK> onValueFunc, Func<Exception, TK> onErrorFunc)
        {
            return !HasValue ? onErrorFunc(Exception) : onValueFunc(Value);
        }

        /// <summary>
        /// Handle both the value and the exception with a delegate. Returning the result of the same type.
        /// </summary>
        /// <typeparam name="TK">Type to return</typeparam>
        /// <param name="onValueFunc">Delegate for the Value</param>
        /// <param name="onErrorFunc">Delegate for the Error</param>
        /// <returns>Result of the newly mapped type</returns>
        public Result<TK> MatchToResult<TK>(Func<K, TK> onValueFunc, Func<Exception, TK> onErrorFunc)
        {
            var value = !HasValue ? onErrorFunc(Exception) : onValueFunc(Value);
            return new Result<TK>(value, null);
        }

        /// <summary>
        /// Handle both the value and the exception with a delegate
        /// </summary>
        /// <param name="onValueFunc">Delegate for the Value</param>
        /// <param name="onErrorFunc">Delegate for the Error</param>
        public void Match(Action<K> onValueFunc, Action<Exception> onErrorFunc)
        {
            if (!HasValue)
                onErrorFunc(Exception);
            else
                onValueFunc(Value);
        }


        /// <summary>
        /// Casts the value to an object and returns the result. Useful since Result of type object will implicitly cast to a result of any T
        /// </summary>
        /// <returns>Result of object</returns>
        public Result<object> ToErrorResult()
        {
            return new Result<object>(null, Exception);
        }

        /// <summary>
        /// Map the value to a new value and return a result leaving the exception untouched
        /// </summary>
        /// <typeparam name="TK">Type to Map the Value To</typeparam>
        /// <param name="valueMapFunc">Mapping Delegate</param>
        /// <returns>Result of new Type</returns>
        public Result<TK> MapValue<TK>(Func<K, TK> valueMapFunc)
        {
            var mappedValue = default(TK);
            if (HasValue)
                mappedValue = valueMapFunc(Value);
            return new Result<TK>(mappedValue, Exception);
        }
    }
}
