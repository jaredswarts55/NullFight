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
        /// Given a method that returns a Result Value if it succeeds, take that value and pass it into another function that returns a result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="toBind"></param>
        /// <param name="errorOnNone"></param>
        /// <returns></returns>
        public static async Task<Result<TV>> Bind<TK, TV>(this Task<RemotingResult<TK>> result, Func<TK, Task<Result<TV>>> toBind, string errorOnNone = null)
        {
            var resultReturned = await result.ToResult().ConfigureAwait(false);
            if (!resultReturned.HasValue)
                return ResultError(errorOnNone ?? "No value passed to method", resultReturned.UnwrapException());
            return await toBind(resultReturned.Unwrap()).ConfigureAwait(false);
        }

        /// <summary>
        /// Given a method that returns a Result Option if it succeeds, take that value and pass it into another function that returns a result.
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="optional"></param>
        /// <param name="toBind"></param>
        /// <param name="errorOnNone"></param>
        /// <returns></returns>
        public static async Task<Result<TV>> BindOptionResult<TK, TV>(this Task<RemotingResult<Option<TK>>> optional, Func<TK, Task<Result<TV>>> toBind, string errorOnNone = null)
        {
            var result = await optional.ToResult().ConfigureAwait(false);
            if (!result.HasValue)
                return result.ToErrorResult();
            if (!result.Unwrap().HasValue)
                return ResultError(errorOnNone ?? "No value passed to method");
            return await toBind(result.Unwrap().Value).ConfigureAwait(false);
        }


        /// <summary>
        /// Convert the RemotingResult to an Result so the full NullFight API is available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Result of T</returns>
        public static async Task<Result<T>> ToResult<T>(this Task<RemotingResult<T>> remotingResult)
        {
            var result = await remotingResult.ConfigureAwait(false);
            return result.ToResult();
        }
    }
}
