using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NullFight.Exceptions;

// ReSharper disable once CheckNamespace
namespace NullFight
{
    /// <summary>
    /// A set of extension methods for use with a NullFight Option or Result
    /// </summary>
    public static partial class FunctionalExtensions
    {
        /// <summary>
        /// Returns an Option with HasValue set to true if the value is not null
        /// </summary>
        /// <param name="value">Value to check for null</param>
        /// <returns>Option of type value</returns>
        public static Option<T> SomeNotNull<T>(T value)
        {
            return new Option<T>(value, value != null);
        }

        /// <summary>
        /// Returns an Option with HasValue set to true if the value is not default(T)
        /// </summary>
        /// <param name="value">Value to check for default</param>
        /// <returns>Option of type value</returns>
        public static Option<T> SomeNotDefault<T>(T value)
        {
            return new Option<T>(value, !value.Equals(default(T)));
        }

        /// <summary>
        /// Returns an Option with HasValue set to true if the someCheckFunc comes back true
        /// </summary>
        /// <param name="value">Value for the Option</param>
        /// <param name="someCheckFunc">Function that sets HasValue</param>
        /// <returns>Option of that value type</returns>
        public static Option<T> SomeIf<T>(T value, Func<T, bool> someCheckFunc)
        {
            return new Option<T>(value, someCheckFunc(value));
        }

        /// <summary>
        /// Returns an Option with HasValue set to true.
        /// </summary>
        /// <param name="value">Value for the Option</param>
        /// <returns>Option with Value</returns>
        public static Option<T> Some<T>(T value)
        {
            return new Option<T>(value, true);
        }

        /// <summary>
        /// Returns an Option with HasValue set to false
        /// </summary>
        /// <returns>Option with a null value and HasValue set to false</returns>
        public static Option<object> None()
        {
            return new Option<object>(null, false);
        }

        /// <summary>
        /// Returns a Result with a value of an Option that has no value.
        /// </summary>
        /// <returns>A Result with a value of 'None'</returns>
        public static Result<Option<T>> NoneResult<T>()
        {
            return new Result<Option<T>>(new Option<T>(default(T), false), null);
        }

        /// <summary>
        /// Returns a Result with a value of an Option with a value
        /// </summary>
        /// <returns>A Result with a value of 'Some'</returns>
        public static Result<Option<T>> SomeResult<T>(T value)
        {
            return new Result<Option<T>>(new Option<T>(value, true), null);
        }

        /// <summary>
        /// Returns a Result with a value of an Option with a value. That Option will be a 'None' if the passed function returns false.
        /// </summary>
        /// <returns>A Result with a value of 'Some' if the functions returns true</returns>
        public static Result<Option<T>> SomeResultIf<T>(T value, Func<T, bool> someCheckFunc)
        {
            return new Result<Option<T>>(new Option<T>(value, someCheckFunc(value)), null);
        }

        /// <summary>
        /// Returns a Result with a value of an Option with a value. That Option will be a 'None' if the value is null.
        /// </summary>
        /// <returns>A Result with a value of 'Some' if the value is not null</returns>
        public static Result<Option<T>> SomeResultNotNull<T>(T value)
        {
            return new Result<Option<T>>(new Option<T>(value, value != null), null);
        }

        /// <summary>
        /// Returns a Result with a value of an Option with a value. That Option will be a 'None' if the value is null.
        /// </summary>
        /// <returns>A Result with a value of 'Some' if the value is not null</returns>
        public static Result<Option<T>> SomeResultNotDefault<T>(T value)
        {
            return new Result<Option<T>>(new Option<T>(value, !value.Equals(default(T))), null);
        }

        /// <summary>
        /// Returns a Option with HasValue set to false. Known as a 'None'
        /// </summary>
        /// <returns>Returns a 'None'</returns>
        public static Option<T> None<T>()
        {
            return new Option<T>(default(T), false);
        }


        /// <summary>
        /// Return a result with a value
        /// </summary>
        /// <typeparam name="T">Type of Result Value</typeparam>
        /// <param name="value">Result Value</param>
        /// <returns>Result of Type</returns>
        public static Result<T> ResultValue<T>(T value)
        {
            return new Result<T>(value, null);
        }

        /// <summary>
        /// Return a result with an exception
        /// </summary>
        /// <typeparam name="T">Type of Result Value</typeparam>
        /// <param name="ex">Exception</param>
        /// <returns>Result of Type With Error</returns>
        public static Result<T> ResultError<T>(Exception ex)
        {
            return new Result<T>(default(T), ex);
        }

        public static Result<object> ResultError(string errorMessage)
        {
            return new Result<object>(null, new FriendlyResultException(errorMessage));
        }

        /// <summary>
        /// Return a result with an exception
        /// </summary>
        /// <typeparam name="T">Type of Result Value</typeparam>
        /// <param name="errorMessage">Exception Message</param>
        /// <returns>Result of Type With Error</returns>
        public static Result<T> ResultError<T>(string errorMessage)
        {
            return new Result<T>(default(T), new FriendlyResultException(errorMessage));
        }


        /// <summary>
        /// Return a result with an exception
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>Result With Error</returns>
        public static Result<object> ResultError(Exception ex)
        {
            return new Result<object>(null, ex);
        }


        /// <summary>
        /// Return a result with an exception
        /// </summary>
        /// <param name="errorMessage">Exception Message</param>
        /// <param name="innerException">Inner Exception</param>
        /// <returns>Result of Type With Error</returns>
        public static Result<object> ResultError(string errorMessage, Exception innerException)
        {
            return new Result<object>(null, new Exception(errorMessage, innerException));
        }

        /// <summary>
        /// Creates a serializable RemotingResult with an Option that has a value
        /// </summary>
        /// <param name="value">Remoting Result with Option with Value</param>
        /// <returns>RemotingResult Some</returns>
        public static RemotingResult<Option<T>> SomeRemotingValue<T>(T value)
        {
            return new RemotingResult<Option<T>>(true, Some(value), null);
        }

        /// <summary>
        /// Creates a serializable RemotingResult with a value with an option inside that has no value.
        /// </summary>
        /// <returns>None inside a RemotingResult</returns>
        public static RemotingResult<Option<T>> NoneRemotingValue<T>()
        {
            return new RemotingResult<Option<T>>(true, new Option<T>(default(T), false), null);
        }

        /// <summary>
        /// Creates a RemotingResult with a Value
        /// </summary>
        /// <param name="value">Value of RemotingResult</param>
        /// <returns>RemotingResult with Value</returns>
        public static RemotingResult<T> RemotingResultValue<T>(T value)
        {
            return new RemotingResult<T>(true, value, null);
        }

        /// <summary>
        /// Creates a RemotingResult with an Error
        /// </summary>
        /// <param name="errorMessage">Error Message</param>
        /// <returns>RemotingResult with Error</returns>
        public static RemotingResult<object> RemotingResultError(string errorMessage)
        {
            return new RemotingResult<object>(true, null, errorMessage);
        }
    }
}
