using Telegram.Bot;

namespace ImportShopBot.Telegram
{
    public class TelegramBotInstance
    {
        private TelegramBotClient TelegramBotClient { get; }

        public TelegramBotInstance(string telegramToken)
        {
            TelegramBotClient = new TelegramBotClient(telegramToken);
            TelegramBotClient.StartReceiving();
        }
        
        
    }
}