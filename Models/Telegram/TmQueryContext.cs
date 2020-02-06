using ImportShopApi.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImportShopApi.Models.Telegram
{
  public class TmQueryContext : TmContext
  {
    public TmQueryContext(
      TmProductService productService,
      TelegramBotClient botClient,
      TmUserService userService,
      CallbackQuery callbackQuery
    ) : base(productService, botClient, userService) => CallbackQuery = callbackQuery;

    public CallbackQuery CallbackQuery { get; }
  }
}