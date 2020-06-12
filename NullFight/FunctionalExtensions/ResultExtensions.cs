using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace NullFight
{
    public static partial class FunctionalExtensions
    {

        /// <summary>
        /// Return the value from the Result or throw the exception inside of it
        /// </summary>
        /// <typeparam name="TV">Result Type</typeparam>
        /// <param name="result">The Result</param>
        /// <param name="message">The issue that occurred if the value can't be retrieved</param>
        /// <returns>Task of Value</returns>
        public static async Task<TV> Expect<TV>(this Task<Result<TV>> result, string message)
        {
            var resultReturned = await result.ConfigureAwait(false);
            return resultReturned.Expect(message);
        }

        /// <summary>
        /// A Bind operation allows you to continue if the Result has a value with another function that returns a result of a different value
        /// </summary>
        /// <param name="result">Result to bind to</param>
        /// <param name="toBind">Function to run if Result has a value</param>
        /// <param name="resultErrorMessage">Error message if the first result has an error</param>
        /// <returns></returns>
        public static async Task<Result<TV>> Bind<TK, TV>(this Task<Result<TK>> result, Func<TK, Task<Result<TV>>> toBind, string resultErrorMessage = null)
        {
            var resultReturned = await result.ConfigureAwait(false);
            if (!resultReturned.HasValue)
                return ResultError(resultErrorMessage ?? "No value passed to method", resultReturned.UnwrapException());
            return await toBind(resultReturned.Unwrap()).ConfigureAwait(false);
        }


        /// <summary>
        /// Handle both the value and the exception with a delegate. Returning the same type.
        /// </summary>
        /// <typeparam name="TV">Type to return</typeparam>
        /// <typeparam name="TO">The Results Value Type</typeparam>
        /// <param name="result">The Result</param>
        /// <param name="onValueFunc">Delegate for the Value</param>
        /// <param name="onErrorFunc">Delegate for the Error</param>
        /// <returns>A new mapped value</returns>
        public static async Task<TV> Match<TO, TV>(this Task<Result<TO>> result, Func<TO, TV> onValueFunc, Func<Exception, TV> onErrorFunc)
        {
            var resultReturned = await result.ConfigureAwait(false);
            return resultReturned.Match(onValueFunc, onErrorFunc);
        }

        /// <summary>
        /// Handle both the value and the exception with a delegate. Returning the same type.
        /// </summary>
        /// <typeparam name="TV">Type to return</typeparam>
        /// <typeparam name="TO">The Results Value Type</typeparam>
        /// <param name="result">The Result</param>
        /// <param name="onValueFunc">Delegate for the Value</param>
        /// <param name="onErrorFunc">Delegate for the Error</param>
        /// <returns>A new mapped value</returns>
        public static async Task<TV> MatchAsync<TO, TV>(this Task<Result<TO>> result, Func<TO, Task<TV>> onValueFunc, Func<Exception, Task<TV>> onErrorFunc)
        {
            var resultReturned = await result.ConfigureAwait(false);
            if (resultReturned.HasValue)
                return await onValueFunc(resultReturned.Unwrap()).ConfigureAwait(false);
            return await onErrorFunc(resultReturned.UnwrapException()).ConfigureAwait(false);
        }

        /// <summary>
        /// If the result has a value, map that value to another with the given function.
        /// </summary>
        /// <param name="result">The Result</param>
        /// <param name="onValueFunc">Function to run on value</param>
        /// <returns>Result of new type</returns>
        public static async Task<Result<TV>> MapValue<TO, TV>(this Task<Result<TO>> result, Func<TO, TV> onValueFunc)
        {
            var resultReturned = await result.ConfigureAwait(false);
            if (resultReturned.HasValue)
                return ResultValue(onValueFunc(resultReturned.Unwrap()));
            return resultReturned.MapValue(x => default(TV));
        }

        /// <summary>
        /// If the result has a value, map that value to another with the given function.
        /// </summary>
        /// <param name="result">The Result</param>
        /// <param name="onValueFunc">Async function to run on value</param>
        /// <returns>Result of new type</returns>
        public static async Task<Result<TV>> MapValueAsync<TO, TV>(this Task<Result<TO>> result, Func<TO, Task<Result<TV>>> onValueFunc)
        {
            var resultReturned = await result.ConfigureAwait(false);
            if (resultReturned.HasValue)
                return await onValueFunc(resultReturned.Unwrap()).ConfigureAwait(false);
            return resultReturned.MapValue(x => default(TV));
        }


        /// <summary>
        /// Return the value from the Result or throw a generic result exception
        /// </summary>
        /// <typeparam name="TV">Result Type</typeparam>
        /// <param name="result">The Result</param>
        /// <returns>Task of Value</returns>
        public static async Task<TV> Unwrap<TV>(this Task<Result<TV>> result)
        {
            var resultReturned = await result.ConfigureAwait(false);
            return resultReturned.Unwrap();
        }



        /// <summary>
        /// This maps a Result object which cannot be serialized with a DataContract serializer due to the Unknown type of 'Exception' to a
        /// RemotingResult that is either a value of type T or a string. This is a lossy conversion, and as a result, there is a provided
        /// logging action to log the Exception preserving things like the stacktrace somewhere.
        /// </summary>
        /// <param name="result">Result to make serializable</param>
        /// <param name="errorLoggingAction">Action to log the Exception</param>
        /// <returns>Serializable RemotingResult</returns>
        public static RemotingResult<TK> ToRemotingResult<TK>(this Result<TK> result, Action<Exception> errorLoggingAction = null)
        {
            var hasValue = result.HasValue;
            TK value;
            string errorMessage = string.Empty;
            if (result.HasValue)
            {
                value = result.Unwrap();
                errorMessage = null;
            }
            else
            {
                var ex = result.UnwrapException();
                if (ex != null)
                {
                    errorLoggingAction?.Invoke(ex);
                    errorMessage = ex.Message;
                }
                value = default(TK);
            }
            return new RemotingResult<TK>(hasValue, value, errorMessage);
        }

    }
}
