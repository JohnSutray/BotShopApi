using System;
using System.Collections.Generic;
using System.Linq;
using ImportShopBot.Controllers.Telegram;
using ImportShopBot.Models.Account;
using ImportShopBot.Models.Telegram;
using Microsoft.Extensions.DependencyInjection;

namespace ImportShopBot.Services
{
    public class TmBotManagerService
    {
        public void UpdateBots()
        {
            var activeAccounts = TmBotInstances.Select(
                bot => AccountService.Accounts.First(account => account.Id == bot.Account.Id)
            );
            var accountsToBootstrap = AccountService.Accounts.AsEnumerable().Except(activeAccounts);
            var accountsToKill = activeAccounts.Except(AccountService.Accounts);

            accountsToBootstrap.ToList().ForEach(BootstrapBot);
            accountsToKill.ToList().ForEach(KillBot);
        }

        private IServiceProvider ServiceProvider { get; }

        private List<TmBotInstance> TmBotInstances { get; } = new List<TmBotInstance>();

        public TmBotManagerService(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
        }

        private TmAccountService AccountService
            => ServiceProvider.GetRequiredService<TmAccountService>();

        private void BootstrapBot(Account account)
        {
            var bot = new TmBotInstance(account, ServiceProvider);
            bot.AddController<TelegramProductController>();
            bot.Start();
            TmBotInstances.Add(bot);
        }

        private void KillBot(Account account) => TmBotInstances
            .First(b => b.Account.Id == account.Id)
            .Stop();
    }
}