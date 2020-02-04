using System.Linq;
using ImportShopBot.Contexts;
using ImportShopBot.Models.Product;

namespace ImportShopBot.Services
{
    public class TmProductService
    {
        private ProductContext ProductContext { get; }
        public int AccountId { get; set; }

        public TmProductService(ProductContext productContext)
            => ProductContext = productContext;

        public IQueryable<Product> Products => ProductContext
            .Products
            .Where(p => p.OwnerId == AccountId);
    }
}