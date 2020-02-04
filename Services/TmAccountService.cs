using System.Linq;
using ImportShopBot.Contexts;
using ImportShopBot.Models.Account;

namespace ImportShopBot.Services
{
    public class TmAccountService
    {
        public IQueryable<Account> Accounts => AccountContext.Accounts;
        private AccountContext AccountContext { get; }

        public TmAccountService(AccountContext accountContext)
            => AccountContext = accountContext;
    }
}