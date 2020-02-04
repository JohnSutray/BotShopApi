using ImportShopBot.Services;
using Telegram.Bot;

namespace ImportShopBot.Models.Telegram
{
    public class TmContext
    {
        public TmProductService ProductService { get; set; }
        public TelegramBotClient TelegramBotClient { get; set; }
        
        public TmUser User { get; set; }
    }
}