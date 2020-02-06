using System.Threading.Tasks;
using ImportShopApi.Contexts;
using ImportShopApi.Models.Account;
using ImportShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ImportShopApi.Services
{
    public class AccountService
    {
        private AccountContext AccountContext { get; }

        public AccountService(AccountContext accountContext) => AccountContext = accountContext;

        public async Task<bool> IsCommonAccountExists(string telegramToken)
            => await FindByToken(telegramToken) != null;

        public async Task<Account> FindByToken(string telegramToken)
            => await AccountContext.Accounts
                .FirstOrDefaultAsync(account => account.TelegramToken == telegramToken);

        public async Task CreateBotAccount(string telegramToken)
        {
            var botAccount = new Account {TelegramToken = telegramToken};
            await AccountContext.AddAsync(botAccount);
            await AccountContext.SaveChangesAsync();
        }

        public async Task RemoveAccount(int ownerId)
        {
            var accountToRemove = await AccountContext.Accounts.FirstAsync(a => a.Id == ownerId);
            AccountContext.Remove(accountToRemove);
            await AccountContext.SaveChangesAsync();
        }
    }
}