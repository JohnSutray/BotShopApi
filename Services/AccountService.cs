using System.Threading.Tasks;
using ImportShopCore;
using ImportShopCore.Attributes;
using ImportShopCore.Models;
using ImportShopCore.Models.Account;
using ImportShopCore.Models.Entities;

namespace ImportShopApi.Services {
  [Service]
  public class AccountService : RepositoryService<Account> {
    public AccountService(ApplicationContext applicationContext)
      : base(applicationContext, context => context.Accounts) { }

    public async Task<bool> CheckIsAccountExistsAsync(string telegramToken) =>
      await ByToken(telegramToken) != null;

    public async Task<Account> ByToken(string telegramToken) =>
      await ByPatternAsync(account => account.TelegramToken == telegramToken);

    public async Task CreateAccount(string telegramToken) {
      var botAccount = new Account {TelegramToken = telegramToken};
      await AddEntityAsync(botAccount);
    }

    public async Task RemoveAccount(int ownerId) => await RemoveByIdAsync(ownerId);
  }
}