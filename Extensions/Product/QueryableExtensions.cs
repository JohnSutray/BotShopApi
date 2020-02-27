using System.Collections.Generic;
using System.Linq;
using ImportShopCore.Extensions.Common;
using ImportShopCore.Models.Product;
using ImportShopApi.Extensions.Common;

namespace ImportShopApi.Extensions.Product {
  public static class QueryableExtensions {
    public static IEnumerable<Category> GetCategories(
      this IEnumerable<ImportShopCore.Models.Product.Product> products
    ) => products.AsEnumerable()
      .GetGroups(p => p.Category)
      .Select(products.GetCategoryWithTypes);

    private static Category GetCategoryWithTypes(
      this IEnumerable<ImportShopCore.Models.Product.Product> products,
      string category
    ) => new Category {
      Name = category,
      Types = products.Where(p => p.Category == category)
        .GetGroups(p => p.Type)
    };
  }
}