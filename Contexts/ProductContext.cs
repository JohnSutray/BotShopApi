using ImportShopBot.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace ImportShopBot.Contexts
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }
    }
}