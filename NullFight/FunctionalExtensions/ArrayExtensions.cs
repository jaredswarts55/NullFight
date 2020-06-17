using System;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


// ReSharper disable once CheckNamespace
namespace NullFight
{
    public static partial class FunctionalExtensions
    {
        /// <summary>
        /// Attempt to retrieve the first item from the collection. If none is found return a None.
        /// </summary>
        /// <param name="source">Source collection</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>None if empty</returns>
        public static Option<TSource> FirstOrOption<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            var first = Enumerable.FirstOrDefault(source, predicate);
            if (first != null)
                return Some(first);
            return None();
        }

        /// <summary>
        /// Attempt to retrieve the first item from the collection. If none is found return a None.
        /// </summary>
        /// <param name="source">Source collection</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>None if empty</returns>
        public static Option<TSource> FirstOrOption<TSource>(
            this IEnumerable<TSource> source)
        {
            var first = Enumerable.FirstOrDefault(source);
            if (first != null)
                return Some(first);
            return None();
        }

        /// <summary>
        /// Attempt to retrieve the first item from the collection. If none is found return a None.
        /// </summary>
        /// <param name="source">Source collection</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>None if empty</returns>
        public static Option<TSource> FirstOrOption<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            var first = Queryable.FirstOrDefault(source, predicate);
            if (first != null)
                return Some(first);
            return None();
        }
    }
}
