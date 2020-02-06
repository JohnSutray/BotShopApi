using ImportShopApi.Services.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImportShopApi.Models.Telegram {
  public class TmMessageContext : TmContext {
    public TmMessageContext(
      TmProductService productService,
      TelegramBotClient botClient,
      TmUserService userService,
      Message message
    ) : base(productService, botClient, userService) => Message = message;

    public Message Message { get; }
  }
}