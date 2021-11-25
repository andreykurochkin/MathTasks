using System;
using System.Collections.Generic;
using System.Linq;

namespace MathTasks.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// returns randomized items from source
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="elementsCount"></param>
        /// <returns></returns>
        public static List<T> ToRandomList<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(arg => Guid.NewGuid())
                .Take(Random.Shared.Next(1, source.Count()))
                .ToList();
        }
    }
}
