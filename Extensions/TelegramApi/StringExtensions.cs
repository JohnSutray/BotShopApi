using System.Threading.Tasks;
using Telegram.Bot;

namespace ImportShopApi.Extensions.TelegramApi {
  public static class StringExtensions {
    public static async Task<bool> CheckTokenIsValidAsync(this string token) {
      try {
        await new TelegramBotClient(token).GetMeAsync();
        return true;
      }
      catch {
        return false;
      }
    }
  }
}