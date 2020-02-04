using System;
using System.Collections.Generic;
using System.Linq;
using ImportShopBot.Models;

namespace ImportShopBot.Extensions.Enumerable
{
    public static partial class EnumerableExtensions
    {
        public static PaginateResult<T> Paginate<T>(
            this IEnumerable<T> items, int page, int limit
        ) => new PaginateResult<T>
        {
            Items = items.Skip(page * limit).Take(limit),
            Limit = limit,
            Page = page,
            TotalPages = (int) Math.Ceiling(items.Count() / (double) limit)
        };
    }
}