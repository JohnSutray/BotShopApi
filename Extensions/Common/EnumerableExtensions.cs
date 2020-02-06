using System;
using System.Collections.Generic;
using System.Linq;
using ImportShopApi.Models;

namespace ImportShopApi.Extensions.Common {
  public static partial class EnumerableExtensions {
    public static IEnumerable<TGroup> GetGroups<TSource, TGroup>(
      this IEnumerable<TSource> source,
      Func<TSource, TGroup> mapper
    ) => source.GroupBy(mapper).Select(g => g.Key);
    
    public static PaginateResult<T> Paginate<T>(
      this IEnumerable<T> items, int page, int limit
    ) => new PaginateResult<T> {
      Items = items.Skip(page * limit).Take(limit).ToArray(),
      Limit = limit,
      Page = page,
      TotalPages = (int) Math.Ceiling(items.Count() / (double) limit)
    };
  }
}