using ImportShopApi.Models.Telegram;
using Microsoft.EntityFrameworkCore;

namespace ImportShopApi.Contexts
{
  public class TmUserContext : DbContext
  {
    public DbSet<TmUser> TmUsers { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    public TmUserContext(DbContextOptions<TmUserContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<TmUser>().HasMany(u => u.CartItems);

      base.OnModelCreating(modelBuilder);
    }
  }
}