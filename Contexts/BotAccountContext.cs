using System;
using ImportShopBot.Models;
using Microsoft.EntityFrameworkCore;

namespace ImportShopBot.Contexts
{
    public class BotAccountContext : DbContext
    {
        public DbSet<BotAccount> BotAccounts { get; set; }

        public BotAccountContext(DbContextOptions<BotAccountContext> options) : base(options)
        {
        }
    }
}