using System;
using System.Threading.Tasks;

namespace NullFight
{
    public static class FunctionalExtensions
    {
        public static Option<T> SomeNotNull<T>(T value)
            where T : class
        {
            return new Option<T>(value, value != null);
        }

        public static Option<T> SomeIf<T>(T value, Func<T, bool> someCheckFunc)
            where T : class
        {
            return new Option<T>(value, someCheckFunc(value));
        }

        public static Option<T> Some<T>(T value)
        {
            return new Option<T>(value, true);
        }

        public static Option<object> None()
        {
            return new Option<object>(null, false);
        }

        public static Result<Option<T>> NoneResult<T>()
        {
            return new Result<Option<T>>(new Option<T>(default(T), false), null);
        }

        public static Result<Option<T>> SomeResult<T>(T value)
        {
            return new Result<Option<T>>(new Option<T>(value, true), null);
        }

        public static Result<Option<T>> SomeResultIf<T>(T value, Func<T, bool> someCheckFunc)
        {
            return new Result<Option<T>>(new Option<T>(value, someCheckFunc(value)), null);
        }

        public static Result<Option<T>> SomeResultNotNull<T>(T value)
            where T : class
        {
            return new Result<Option<T>>(new Option<T>(value, value != null), null);
        }

        public static Option<T> None<T>()
        {
            return new Option<T>(default(T), false);
        }

        public static async Task<T> ValueOr<T>(this Task<Option<T>> optionTask, T valueIfNone)
        {
            var option = await optionTask;
            return option.ValueOr(valueIfNone);
        }

        public static async Task<TK> Match<T, TK>(this Task<Option<T>> optionTask, Func<T, TK> onValueFunc, Func<TK> onNone)
        {
            var option = await optionTask;
            return option.Match(onValueFunc, onNone);
        }

        public static async Task Match<T>(this Task<Option<T>> optionTask, Action<T> onValueFunc, Action onNone)
        {
            var option = await optionTask;
            option.Match(onValueFunc, onNone);
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

        /// <summary>
        /// Return a result with an exception
        /// </summary>
        /// <typeparam name="T">Type of Result Value</typeparam>
        /// <param name="errorMessage">Exception Message</param>
        /// <returns>Result of Type With Error</returns>
        public static Result<T> ResultError<T>(string errorMessage)
        {
            return new Result<T>(default(T), new Exception(errorMessage));
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
        /// <returns>Result of Type With Error</returns>
        public static Result<object> ResultError(string errorMessage)
        {
            return new Result<object>(null, new Exception(errorMessage));
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
        /// Return the value from the Result or throw the exception inside of it
        /// </summary>
        /// <typeparam name="TV">Result Type</typeparam>
        /// <param name="resultObject">The Result</param>
        /// <param name="message">The issue that occurred if the value can't be retrieved</param>
        /// <param name="throwExceptionOnError">Whether or not to throw the exception on error</param>
        /// <returns>Task of Value</returns>
        public static async Task<TV> Expect<TV>(this Task<Result<TV>> resultObject, string message, bool throwExceptionOnError = true)
        {
            var result = await resultObject;
            return result.Expect(message);
        }
        public static async Task<TV> ExpectOptionValue<TV>(this Task<Result<Option<TV>>> resultObject, string resultMessage, string optionMissingMessage = "")
        {
            var result = await resultObject;
            if (!result.HasValue)
                throw new Exception(resultMessage, result.UnwrapException());
            if (!result.Unwrap().HasValue)
                throw new Exception(string.IsNullOrWhiteSpace(optionMissingMessage) ? resultMessage : optionMissingMessage);
            return result.Unwrap().Value;
        }


        /// <summary>
        /// Return the value from the Result or throw a generic result exception
        /// </summary>
        /// <typeparam name="TV">Result Type</typeparam>
        /// <param name="resultObject">The Result</param>
        /// <returns>Task of Value</returns>
        public static async Task<TV> Unwrap<TV>(this Task<Result<TV>> resultObject)
        {
            var result = await resultObject;
            return result.Unwrap();
        }

        /// <summary>
        /// Handle both the value and the exception with a delegate. Returning the same type.
        /// </summary>
        /// <typeparam name="TV">Type to return</typeparam>
        /// <typeparam name="TO">The Results Value Type</typeparam>
        /// <param name="resultObject">The Result</param>
        /// <param name="onValueFunc">Delegate for the Value</param>
        /// <param name="onErrorFunc">Delegate for the Error</param>
        /// <returns>A new mapped value</returns>
        public static async Task<TV> Match<TO, TV>(this Task<Result<TO>> resultObject, Func<TO, TV> onValueFunc, Func<Exception, TV> onErrorFunc)
        {
            var result = await resultObject;
            return result.Match(onValueFunc, onErrorFunc);
        }

        /// <summary>
        /// Run a 
        /// </summary>
        /// <typeparam name="TO"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="resultObject"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<Result<TV>> ThenAsync<TO, TV>(this Task<Result<TO>> resultObject, Func<TO, Task<Result<TV>>> func)
        {
            var result = await resultObject;
            if (!result.HasValue)
                return result.MapValue(x => default(TV));
            return await func(result.Unwrap());
        }
    }
}
