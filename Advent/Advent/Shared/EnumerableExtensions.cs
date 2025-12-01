using System.Collections.Generic;

namespace Advent.Shared
{
    public static class EnumerableExtensions
    {
        public static Dictionary<T, int> ToHistogram<T>(this IEnumerable<T> input) where T : notnull
        {
            var dict = new Dictionary<T, int>();
            foreach (var t in input)
            {
                if (!dict.TryGetValue(t, out var count))
                    count = 0;
                dict[t] = count + 1;
            }
            return dict;
        }
    }
}
