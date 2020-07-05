using System.Collections.Generic;
using System.Linq;
using BotShopCore.Extensions.Common;
using BotShopCore.Models;

namespace BotShopApi.Extensions.Product {
  public static class QueryableExtensions {
    public static IEnumerable<Category> GetCategories(
      this IEnumerable<BotShopCore.Models.Entities.Product> products
    ) => products.AsEnumerable()
      .GetGroups(p => p.Category)
      .Select(products.GetCategoryWithTypes);

    private static Category GetCategoryWithTypes(
      this IEnumerable<BotShopCore.Models.Entities.Product> products,
      string category
    ) => new Category {
      Name = category,
      Types = products.Where(p => p.Category == category)
        .GetGroups(p => p.Type)
        .ToList()
    };
  }
}