using System;
using System.Collections.Generic;
using System.Text;
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
        /// Converts a ResultOption error to a optional None
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        /// <param name="resultOption"></param>
        /// <returns></returns>
        public static async Task<Option<TV>> MapResultErrorToNone<TV>(this Task<Result<Option<TV>>> resultOption)
        {
            var resultReturned = await resultOption.ConfigureAwait(false);
            return resultReturned.Match(x => x, e => None<TV>());
        }


        /// <summary>
        /// Converts a ResultOption that contains a None to a ResultError with the given message
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        /// <param name="resultOption"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static async Task<Result<TV>> MapNoneToResultError<TV>(this Task<Result<Option<TV>>> resultOption, string errorMessage)
        {
            var resultReturned = await resultOption.ConfigureAwait(false);
            if (!resultReturned.HasValue)
                return resultReturned.MapValue(x => default(TV));
            return resultReturned.Unwrap().HasValue ? resultReturned.MapValue(y => y.Value) : ResultError<TV>(errorMessage);
        }


        /// <summary>
        /// Converts a ResultOption to a value. Throwing an exception with the given message.
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        /// <param name="resultOption">Result Option</param>
        /// <param name="resultErrorMessage">Message if result was in error</param>
        /// <param name="optionalNoneMessage">Message if optional was None</param>
        /// <returns></returns>
        public static async Task<TV> ExpectOptionValue<TV>(this Task<Result<Option<TV>>> resultOption, string resultErrorMessage, string optionalNoneMessage = "")
        {
            var resultReturned = await resultOption.ConfigureAwait(false);
            if (!resultReturned.HasValue)
                resultReturned.Expect(resultErrorMessage);
            if (!resultReturned.Unwrap().HasValue)
            {
                throw new FriendlyResultException(string.IsNullOrWhiteSpace(optionalNoneMessage) ? resultErrorMessage : optionalNoneMessage);
            }
            return resultReturned.Unwrap().Value;
        }

        /// <summary>
        /// Maps the Options Value if both the resultOption and the option have a value.
        /// </summary>
        /// <returns></returns>
        public static async Task<Result<Option<TV>>> MapOption<TK, TV>(this Task<Result<Option<TK>>> resultOption, Func<TK, TV> optionalValueMap)
        {
            var resultReturned = await resultOption.ConfigureAwait(false);
            if (!resultReturned.HasValue)
                return resultReturned.MapValue(x => default(Option<TV>));
            return resultReturned.MapValue(x => x.MapValue(optionalValueMap));
        }

        /// <summary>
        /// Given a method that returns a Result Option if it succeeds, take that value and pass it into another function that returns a resultOption.
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="resultOption"></param>
        /// <param name="toBind"></param>
        /// <param name="errorOnNone"></param>
        /// <returns></returns>
        public static async Task<Result<TV>> BindOptionResult<TK, TV>(this Task<Result<Option<TK>>> resultOption, Func<TK, Task<Result<TV>>> toBind, string errorOnNone = null)
        {
            var resultReturned = await resultOption.ConfigureAwait(false);
            if (!resultReturned.HasValue)
                return resultReturned.ToErrorResult();
            if (!resultReturned.Unwrap().HasValue)
                return ResultError(new FriendlyResultException(errorOnNone ?? "No value passed to method"));
            return await toBind(resultReturned.Unwrap().Value).ConfigureAwait(false);
        }

    }
}
