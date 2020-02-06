using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImportShopApi.Attributes;
using ImportShopApi.Services;
using ImportShopApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace ImportShopApi.Models.Telegram
{
    public class TmBotInstance
    {
        public TmBotInstance(Account.Account account, IServiceScopeFactory scopeFactory)
        {
            Account = account;
            ScopeFactory = scopeFactory;
            RootScope = scopeFactory.CreateScope();
            Logger = RootScope.ServiceProvider.GetRequiredService<ILogger<TmBotInstance>>();
        }

        public Account.Account Account { get; }
        private TelegramBotClient BotClient { get; set; }
        private IServiceScopeFactory ScopeFactory { get; }
        private IEnumerable<TmHandlerContainer<TmMessageContext>> MessageHandlers { get; set; }
        private IEnumerable<TmHandlerContainer<TmQueryContext>> CallbackQueryHandlers { get; set; }
        private ILogger<TmBotInstance> Logger { get; }
        private IServiceScope RootScope { get; }
        private async void HandleMessage(object sender, MessageEventArgs args) => await RootMessageHandler(args);
        private async void HandleQuery(object sender, CallbackQueryEventArgs args) => await RootQueryHandler(args);

        public void Start()
        {
            BotClient = new TelegramBotClient(Account.TelegramToken);
            BotClient.OnMessage += HandleMessage;
            BotClient.OnCallbackQuery += HandleQuery;
            BotClient.StartReceiving();
        }

        public void Stop()
        {
            BotClient.StopReceiving();
            RootScope.Dispose();
        }

        public TmBotInstance AddController<T>() where T : new()
        {
            var controllerInstance = new T();

            MessageHandlers = controllerInstance
                .GetType()
                .GetMethodsWithAttribute<TmMessageHandler>()
                .Select(m => m.ToTmContainer<TmMessageContext>(controllerInstance));

            CallbackQueryHandlers = controllerInstance
                .GetType()
                .GetMethodsWithAttribute<TmQueryHandler>()
                .Select(m => m.ToTmContainer<TmQueryContext>(controllerInstance));

            return this;
        }

        private async Task RootMessageHandler(MessageEventArgs messageArgs)
        {
            using var scope = ScopeFactory.CreateScope();
            var productService = scope.ServiceProvider.GetRequiredService<TmProductService>();
            var userService = scope.ServiceProvider.GetRequiredService<TmUserService>();
            productService.AccountId = Account.Id;
            userService.AccountId = Account.Id;

            await HandleTmEvent(
                new TmMessageContext(productService, BotClient, userService, messageArgs.Message),
                MessageHandlers,
                messageArgs.Message.Text
            );
        }

        private async Task RootQueryHandler(CallbackQueryEventArgs queryEventArgs)
        {
            using var scope = ScopeFactory.CreateScope();
            var productService = scope.ServiceProvider.GetRequiredService<TmProductService>();
            var userService = scope.ServiceProvider.GetRequiredService<TmUserService>();
            productService.AccountId = Account.Id;
            userService.AccountId = Account.Id;

            await HandleTmEvent(
                new TmQueryContext(
                    productService,
                    BotClient,
                    userService,
                    queryEventArgs.CallbackQuery
                ),
                CallbackQueryHandlers,
                queryEventArgs.CallbackQuery.Data
            );
        }

        private async Task HandleTmEvent<TContext>(
            TContext context,
            IEnumerable<TmHandlerContainer<TContext>> handlers,
            string content
        ) where TContext : TmContext
        {
            foreach (var handler in handlers)
                if (handler.HandleBy.IsMatch(content) && await handler.Handler(context))
                    return;


            Logger.LogError("There is no suitable handler to process update.");
        }
    }
}