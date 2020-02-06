using ImportShopApi.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace ImportShopApi.Contexts
{
    public class AccountContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }
    }
}