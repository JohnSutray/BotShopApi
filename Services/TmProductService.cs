using System.Linq;
using ImportShopApi.Contexts;
using ImportShopApi.Models.Product;

namespace ImportShopApi.Services
{
  public class TmProductService
  {
    private ProductContext ProductContext { get; }
    public int AccountId { private get; set; }

    public TmProductService(ProductContext productContext)
      => ProductContext = productContext;

    public IQueryable<Product> Products => ProductContext
      .Products
      .Where(p => p.OwnerId == AccountId);
  }
}