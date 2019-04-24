using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NullFight.Exceptions;

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
        public static IEnumerable<T> FilterAndMapValues<T>(this IEnumerable<Option<T>> options)
        {
            return options.Where(x => x.HasValue).Select(x => x.Value);
        }
        public static async Task<T> ValueOr<T>(this Task<Option<T>> optionTask, T valueIfNone)
        {
            var option = await optionTask;
            return option.ValueOr(valueIfNone);
        }

        public static async Task<T> Expect<T>(this Task<Option<T>> optionTask, string message)
        {
            var option = await optionTask;
            if (option.HasValue)
                return option.Value;
            throw new Exception(message);
        }

        public static async Task<T> Expect<T>(this Task<Option<T>> optionTask, Exception ex)
        {
            var option = await optionTask;
            if (option.HasValue)
                return option.Value;
            throw ex;
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


        public static async Task<Option<TK>> MapValue<T, TK>(this Task<Option<T>> optionTask, Func<T, TK> onValueFunc)
        {
            var option = await optionTask;
            return option.MapValue(onValueFunc);
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
        /// Return the value from the Result or throw the exception inside of it
        /// </summary>
        /// <typeparam name="TV">Result Type</typeparam>
        /// <param name="resultObject">The Result</param>
        /// <param name="message">The issue that occurred if the value can't be retrieved</param>
        /// <param name="throwExceptionOnError">Whether or not to throw the exception on error</param>
        /// <returns>Task of Value</returns>
        public static async Task<TV> Expect<TV>(this Task<Result<TV>> resultObject, string message)
        {
            var result = await resultObject;
            return result.Expect(message);
        }

        public static async Task<Result<TV>> Bind<TK, TV>(this Task<Result<TK>> resultObject, Func<TK, Task<Result<TV>>> toBind, string errorOnNone = null)
        {
            var result = await resultObject;
            if (!result.HasValue)
                return ResultError(errorOnNone ?? "No value passed to method", result.UnwrapException());
            return await toBind(result.Unwrap());
        }

        public static async Task<Result<TV>> BindOption<TK, TV>(this Task<Option<TK>> optionObject, Func<TK, Task<Result<TV>>> toBind, string errorOnNone = null)
        {
            var result = await optionObject;
            if (!result.HasValue)
                return ResultError(new FriendlyResultException(errorOnNone ?? "No value present in option"));
            return await toBind(result.Value);
        }

        /// <summary>
        /// Given a method that returns a Result Option if it succeeds, take that value and pass it into another function that returns a result.
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="optionObject"></param>
        /// <param name="toBind"></param>
        /// <param name="errorOnNone"></param>
        /// <returns></returns>
        public static async Task<Result<TV>> BindOptionResult<TK, TV>(this Task<Result<Option<TK>>> optionObject, Func<TK, Task<Result<TV>>> toBind, string errorOnNone = null)
        {
            var result = await optionObject;
            if (!result.HasValue)
                return result.ToErrorResult();
            if (!result.Unwrap().HasValue)
                return ResultError(new FriendlyResultException(errorOnNone ?? "No value passed to method"));
            return await toBind(result.Unwrap().Value);
        }

        public static async Task<Result<TV>> Bind<TK, TV>(this Task<RemotingResult<TK>> resultObject, Func<TK, Task<Result<TV>>> toBind, string errorOnNone = null)
        {
            var result = await resultObject.ToResult();
            if (!result.HasValue)
                return ResultError(errorOnNone ?? "No value passed to method", result.UnwrapException());
            return await toBind(result.Unwrap());
        }

        /// <summary>
        /// Given a method that returns a Result Option if it succeeds, take that value and pass it into another function that returns a result.
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="optionObject"></param>
        /// <param name="toBind"></param>
        /// <param name="errorOnNone"></param>
        /// <returns></returns>
        public static async Task<Result<TV>> BindOptionResult<TK, TV>(this Task<RemotingResult<Option<TK>>> optionObject, Func<TK, Task<Result<TV>>> toBind, string errorOnNone = null)
        {
            var result = await optionObject.ToResult();
            if (!result.HasValue)
                return result.ToErrorResult();
            if (!result.Unwrap().HasValue)
                return ResultError(errorOnNone ?? "No value passed to method");
            return await toBind(result.Unwrap().Value);
        }

        /// <summary>
        /// Converts a Result Option that contains a None to a ResultError with the given message
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        /// <param name="resultObject"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static async Task<Result<TV>> MapNoneToResultError<TV>(this Task<Result<Option<TV>>> resultObject, string errorMessage)
        {
            var result = await resultObject;
            if (!result.HasValue)
                return result.MapValue(x => default(TV));
            return result.Unwrap().HasValue ? result.MapValue(y => y.Value) : ResultError<TV>(errorMessage);
        }

        /// <summary>
        /// Converts a result error to a None
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        /// <param name="resultObject"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static async Task<Option<TV>> MapResultErrorToNone<TV>(this Task<Result<Option<TV>>> resultObject)
        {
            var result = await resultObject;
            return result.Match(x => x, e => None<TV>());
        }

        public static async Task<TV> ExpectOptionValue<TV>(this Task<Result<Option<TV>>> resultObject, string resultMessage, string optionMissingMessage = "")
        {
            var result = await resultObject;
            if (!result.HasValue)
                result.Expect(resultMessage);
            if (!result.Unwrap().HasValue)
            {
                throw new FriendlyResultException(string.IsNullOrWhiteSpace(optionMissingMessage) ? resultMessage : optionMissingMessage);
            }
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

        public static async Task<TV> MatchAsync<TO, TV>(this Task<Result<TO>> resultObject, Func<TO, Task<TV>> onValueFunc, Func<Exception, Task<TV>> onErrorFunc)
        {
            var result = await resultObject;
            if (result.HasValue)
                return await onValueFunc(result.Unwrap());
            return await onErrorFunc(result.UnwrapException());
        }

        public static async Task<Result<TV>> MapValue<TO, TV>(this Task<Result<TO>> resultObject, Func<TO, TV> onValueFunc)
        {
            var result = await resultObject;
            if (result.HasValue)
                return ResultValue(onValueFunc(result.Unwrap()));
            return result.MapValue(x => default(TV));
        }

        public static async Task<Result<TV>> MapValueAsync<TO, TV>(this Task<Result<TO>> resultObject, Func<TO, Task<Result<TV>>> onValueFunc)
        {
            var result = await resultObject;
            if (result.HasValue)
                return await onValueFunc(result.Unwrap());
            return result.MapValue(x => default(TV));
        }

        public static async Task<Result<T>> ToResult<T>(this Task<RemotingResult<T>> resultObject)
        {
            var result = await resultObject;
            return result.ToResult();
        }

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

        public static RemotingResult<Option<T>> SomeRemotingValue<T>(T value)
        {
            return new RemotingResult<Option<T>>(true, Some(value), null);
        }

        public static RemotingResult<Option<T>> NoneRemotingValue<T>()
        {
            return new RemotingResult<Option<T>>(true, new Option<T>(default(T), false), null);
        }


        public static RemotingResult<T> RemotingResultValue<T>(T value)
        {
            return new RemotingResult<T>(true, value, null);
        }
        public static RemotingResult<object> RemotingResultError(string errorMessage)
        {
            return new RemotingResult<object>(true, null, errorMessage);
        }
    }
}
