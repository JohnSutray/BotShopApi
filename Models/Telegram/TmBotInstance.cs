using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImportShopBot.Attributes;
using ImportShopBot.Extensions;
using ImportShopBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace ImportShopBot.Models.Telegram
{
    public class TmBotInstance
    {
        public Account.Account Account { get; }
        private TelegramBotClient BotClient { get; set; }
        private IServiceProvider ServiceProvider { get; }
        private IEnumerable<TmHandlerContainer<TmMessageContext>> MessageHandlers { get; set; }
        private IEnumerable<TmHandlerContainer<TmQueryContext>> CallbackQueryHandlers { get; set; }

        private TmProductService ProductService
        {
            get
            {
                var service = ServiceProvider.GetRequiredService<TmProductService>();

                service.AccountId = Account.Id;

                return service;
            }
        }

        private TmUserService UserService => ServiceProvider.GetRequiredService<TmUserService>();
        private ILogger<TmBotInstance> Logger => ServiceProvider.GetRequiredService<ILogger<TmBotInstance>>();

        public TmBotInstance(Account.Account account, IServiceProvider serviceProvider)
        {
            Account = account;
            ServiceProvider = serviceProvider;
        }

        private void HandleMessage(object sender, MessageEventArgs args) => RootMessageHandler(args);
        private void HandleQuery(object sender, CallbackQueryEventArgs args) => RootQueryHandler(args);

        public void Start()
        {
            BotClient = new TelegramBotClient(Account.TelegramToken);
            BotClient.OnMessage += HandleMessage;
            BotClient.OnCallbackQuery += HandleQuery;
            BotClient.StartReceiving();
        }

        public void Stop() => BotClient.StopReceiving();

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

        private async void RootMessageHandler(MessageEventArgs messageArgs) => HandleTmEvent(
            await CreateMessageContext(messageArgs.Message),
            MessageHandlers,
            messageArgs.Message.Text
        );

        private async void RootQueryHandler(CallbackQueryEventArgs queryEventArgs) => HandleTmEvent(
            await CreateQueryContext(queryEventArgs.CallbackQuery),
            CallbackQueryHandlers,
            queryEventArgs.CallbackQuery.Data
        );

        private async void HandleTmEvent<TContext>(
            TContext context,
            IEnumerable<TmHandlerContainer<TContext>> handlers,
            string content
        ) where TContext : TmContext
        {
            Logger.LogDebug("Event handle start");
            LogUserState(context, content);

            foreach (var handler in handlers)
                if (handler.HandleBy.IsMatch(content) && await handler.Handler(context))
                {
                    await UserService.SaveChanges();
                    Logger.LogDebug("Event handled");
                    LogUserState(context, content);
                    return;
                }


            Logger.LogError("There is no suitable handler to process update.");
            LogUserState(context, content);
        }

        private void LogUserState(TmContext context, string content) => Logger.LogDebug(
            $"Chat state: {context.User.ChatState}\n\t" +
            $"Message: {content}"
        );

        private async Task<TmMessageContext> CreateMessageContext(Message message) => new TmMessageContext
        {
            Message = message,
            ProductService = ProductService,
            User = await UserService.GetUserByTmId(message.From.Id, Account.Id),
            TelegramBotClient = BotClient
        };

        private async Task<TmQueryContext> CreateQueryContext(CallbackQuery query) => new TmQueryContext
        {
            CallbackQuery = query,
            ProductService = ProductService,
            User = await UserService.GetUserByTmId(query.From.Id, Account.Id),
            TelegramBotClient = BotClient,
        };
    }
}