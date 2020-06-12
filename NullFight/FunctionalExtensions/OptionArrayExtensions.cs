using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NullFight
{
    public static partial class FunctionalExtensions
    {
        /// <summary>
        /// Given the array of Options, run a method on each option with a value and return only the ones that
        /// had values.
        /// </summary>
        /// <param name="options">Enumerable of Options</param>
        /// <returns>Returns a filter list of Options that have been mapped</returns>
        public static IEnumerable<T> FilterAndMapValues<T>(this IEnumerable<Option<T>> options)
        {
            return options.Where(x => x.HasValue).Select(x => x.Value);
        }
    }
}
