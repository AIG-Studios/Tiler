using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq.Extensions
{
    public static class EnumerableHelper<E>
    {
        private static System.Random r;

        static EnumerableHelper()
        {
            r = new System.Random();
        }

        public static T Random<T>(IEnumerable<T> input)
        {
            if (input.Count() == 0) return default(T);
            return input.ElementAt(r.Next(input.Count()));
        }

        public static T Random<T>(IEnumerable<T> input, System.Random random)
        {
            if (input.Count() == 0) return default(T);
            return input.ElementAt(random.Next(input.Count()));
        }
    }

    public static class EnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> input)
        {
            return EnumerableHelper<T>.Random(input);
        }

        public static T Random<T>(this IEnumerable<T> input, System.Random random)
        {
            return EnumerableHelper<T>.Random(input, random);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> input)
        {
            return input.OrderBy(x => Guid.NewGuid());
        }

        public static IEnumerable<T> Without<T>(this IEnumerable<T> input, T value)
        {
            return input.Where(x => !System.Object.Equals(x, value));
        }
    }
}