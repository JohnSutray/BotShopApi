using System;
using System.Collections.Generic;
using System.Linq;

namespace ImportShopBot.Extensions.Enumerable
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<TGroup> GetGroups<TSource, TGroup>(
            this IEnumerable<TSource> source,
            Func<TSource, TGroup> mapper
        ) => source.GroupBy(mapper).Select(g => g.Key);
    }
}