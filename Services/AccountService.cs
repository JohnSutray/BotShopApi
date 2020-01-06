using System.Threading.Tasks;
using ImportShopBot.Contexts;
using ImportShopBot.Models;
using Microsoft.EntityFrameworkCore;

namespace ImportShopBot.Services
{
    public class AccountService
    {
        private BotAccountContext BotAccountContext { get; }

        public AccountService(BotAccountContext botAccountContext) => BotAccountContext = botAccountContext;

        public async Task<bool> IsCommonAccountExists(string telegramToken)
            => await FindByToken(telegramToken) != null;

        public async Task<BotAccount> FindByToken(string telegramToken)
            => await BotAccountContext.BotAccounts
                .FirstOrDefaultAsync(account => account.TelegramToken == telegramToken);

        public async Task CreateBotAccount(string telegramToken)
        {
            var botAccount = new BotAccount {TelegramToken = telegramToken};
            await BotAccountContext.AddAsync(botAccount);
            await BotAccountContext.SaveChangesAsync();
        }

        public async Task RemoveBotAccount(int ownerId)
        {
            var accountToRemove = await BotAccountContext.BotAccounts.FirstAsync(a => a.Id == ownerId);
            BotAccountContext.Remove(accountToRemove);
            await BotAccountContext.SaveChangesAsync();
        }
    }
}