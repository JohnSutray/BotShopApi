using System.Threading.Tasks;
using BotShopCore;
using BotShopCore.Attributes;
using BotShopCore.Models;
using BotShopCore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BotShopApi.Services {
  [Service]
  public class AccountService : RepositoryService<Account> {
    public AccountService(ApplicationContext applicationContext)
      : base(applicationContext) { }

    public async Task<bool> CheckIsAccountExistsAsync(string telegramToken) =>
      await ByToken(telegramToken) != null;

    public async Task<Account> ByToken(string telegramToken) =>
      await ByPatternAsync(account => account.TelegramToken == telegramToken);

    public async Task CreateAccount(string telegramToken) {
      var botAccount = new Account {TelegramToken = telegramToken};
      await AddEntityAsync(botAccount);
    }

    public async Task RemoveAccount(int ownerId) => await RemoveByIdAsync(ownerId);
    protected override DbSet<Account> Set => Context.Accounts;
  }
}
