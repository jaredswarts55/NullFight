using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NullFight.Exceptions;

// ReSharper disable once CheckNamespace
namespace NullFight
{
    public static partial class FunctionalExtensions
    {
        
        /// <summary>
        /// Returns the value from the Option if there was one, or the default passed in as a second parameter.
        /// </summary>
        /// <param name="optionTask">Task that returns an Option</param>
        /// <param name="valueIfNone">The value to use if the Option has no value</param>
        /// <returns>Task of Option Value or default</returns>
        public static async Task<T> ValueOr<T>(this Task<Option<T>> optionTask, T valueIfNone)
        {
            var option = await optionTask.ConfigureAwait(false);
            return option.ValueOr(valueIfNone);
        }

        /// <summary>
        /// Returns the value, throwing an exception with the given message if there was no value.
        /// </summary>
        /// <param name="optionTask">The Option</param>
        /// <param name="message">Error message in thrown exception</param>
        /// <returns>Value of option</returns>
        public static async Task<T> Expect<T>(this Task<Option<T>> optionTask, string message)
        {
            var option = await optionTask.ConfigureAwait(false);
            if (option.HasValue)
                return option.Value;
            throw new Exception(message);
        }

        /// <summary>
        /// Returns the value, throwing an exception with the given message if there was no value.
        /// </summary>
        /// <param name="optionTask">The Option</param>
        /// <param name="ex">Exception to throw</param>
        /// <returns>Value of option</returns>
        public static async Task<T> Expect<T>(this Task<Option<T>> optionTask, Exception ex)
        {
            var option = await optionTask.ConfigureAwait(false);
            if (option.HasValue)
                return option.Value;
            throw ex;
        }

        /// <summary>
        /// Handle both the value and a None with a delegate. Returning the same type.
        /// </summary>
        /// <param name="optionTask">The Option</param>
        /// <param name="onValueFunc">Delegate for the Value</param>
        /// <param name="onNone">Delegate for the None</param>
        /// <returns>Value returned from delegates</returns>
        public static async Task<TK> Match<T, TK>(this Task<Option<T>> optionTask, Func<T, TK> onValueFunc, Func<TK> onNone)
        {
            var option = await optionTask.ConfigureAwait(false);
            return option.Match(onValueFunc, onNone);
        }

        /// <summary>
        /// Handle both the value and a None with a delegate. Returning nothing.
        /// </summary>
        /// <param name="optionTask">The Option</param>
        /// <param name="onValueFunc">Delegate for the Value</param>
        /// <param name="onNone">Delegate for the None</param>
        public static async Task Match<T>(this Task<Option<T>> optionTask, Action<T> onValueFunc, Action onNone)
        {
            var option = await optionTask.ConfigureAwait(false);
            option.Match(onValueFunc, onNone);
        }

        /// <summary>
        /// Maps the value of the option if there is one to another value using the given onValueFunc.
        /// </summary>
        /// <param name="optionTask">Option to map</param>
        /// <param name="onValueFunc">Function to map the value</param>
        /// <returns>Mapped Value</returns>
        public static async Task<Option<TK>> MapValue<T, TK>(this Task<Option<T>> optionTask, Func<T, TK> onValueFunc)
        {
            var option = await optionTask.ConfigureAwait(false);
            return option.MapValue(onValueFunc);
        }


        /// <summary>
        /// A Bind operation allows you to continue if the option has a value with another function that returns a result
        /// </summary>
        /// <param name="optional">The option being bound</param>
        /// <param name="toBind">The function to run if there is a value with the options value as an argument</param>
        /// <param name="errorOnNone">The error to return in the result if there is no value</param>
        /// <returns>A result of the toBind function or an error result</returns>
        public static async Task<Result<TV>> BindOption<TK, TV>(this Task<Option<TK>> optional, Func<TK, Task<Result<TV>>> toBind, string errorOnNone = null)
        {
            var result = await optional.ConfigureAwait(false);
            if (!result.HasValue)
                return ResultError(new FriendlyResultException(errorOnNone ?? "No value present in option"));
            return await toBind(result.Value).ConfigureAwait(false);
        }

    }
}
