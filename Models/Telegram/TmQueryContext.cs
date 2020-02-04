using Telegram.Bot.Types;

namespace ImportShopBot.Models.Telegram
{
    public class TmQueryContext : TmContext
    {
        public CallbackQuery CallbackQuery { get; set; }
    }
}