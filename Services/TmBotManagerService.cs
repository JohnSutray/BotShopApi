using System;
using System.Collections.Generic;
using System.Linq;
using ImportShopApi.Controllers.Telegram;
using ImportShopApi.Models.Account;
using ImportShopApi.Models.Telegram;
using Microsoft.Extensions.DependencyInjection;

namespace ImportShopApi.Services {
  public class TmBotManagerService {
    public TmBotManagerService(IServiceScopeFactory serviceScopeFactory) {
      ScopeFactory = serviceScopeFactory;
      RootScope = serviceScopeFactory.CreateScope();
      AccountService = RootScope.ServiceProvider.GetRequiredService<TmAccountService>();
    }

    private IServiceScope RootScope { get; }
    private TmAccountService AccountService { get; }
    private IServiceScopeFactory ScopeFactory { get; }
    private List<TmBotInstance> TmBotInstances { get; } = new List<TmBotInstance>();

    public void UpdateBots() {
      var activeAccounts = TmBotInstances.Select(
        bot => AccountService.Accounts.First(account => account.Id == bot.Account.Id)
      );
      var accountsToBootstrap = AccountService.Accounts.AsEnumerable().Except(activeAccounts);
      var accountsToKill = activeAccounts.Except(AccountService.Accounts);

      accountsToBootstrap.ToList().ForEach(BootstrapBot);
      accountsToKill.ToList().ForEach(KillBot);
    }

    private void BootstrapBot(Account account) {
      var bot = new TmBotInstance(account, ScopeFactory);
      bot.AddController<TelegramProductController>();
      bot.Start();
      TmBotInstances.Add(bot);
    }

    private void KillBot(Account account) => TmBotInstances
      .First(b => b.Account.Id == account.Id)
      .Stop();
  }
}