using ImportShopApi.Services;
using Telegram.Bot;

namespace ImportShopApi.Models.Telegram {
  public class TmContext {
    public TmContext(
      TmProductService productService,
      TelegramBotClient botClient,
      TmUserService userService
    ) {
      ProductService = productService;
      TelegramBotClient = botClient;
      UserService = userService;
    }

    public TmProductService ProductService { get; }
    public TelegramBotClient TelegramBotClient { get; }
    public TmUserService UserService { get; }
  }
}