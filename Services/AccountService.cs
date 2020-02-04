using System.Threading.Tasks;
using ImportShopBot.Contexts;
using ImportShopBot.Models;
using ImportShopBot.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace ImportShopBot.Services
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