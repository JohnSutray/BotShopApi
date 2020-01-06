using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ImportShopBot.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> enumerable, int page, int limit)
            => enumerable.Skip((page - 1) * limit).Take(limit);

        public async static Task<IEnumerable<T>> PaginateAndFilterAsync<T>(
            this IQueryable<T> queryable,
            int page,
            int limit,
            Func<T, bool> predicate
        ) => await queryable
            .Where(p => predicate(p))
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
    }
}