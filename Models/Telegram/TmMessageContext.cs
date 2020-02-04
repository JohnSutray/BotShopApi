using Telegram.Bot.Types;

namespace ImportShopBot.Models.Telegram
{
    public class TmMessageContext : TmContext
    {
        public Message Message { get; set; }
    }
}