using ImportShopBot.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace ImportShopBot.Contexts
{
    public class AccountContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }
    }
}