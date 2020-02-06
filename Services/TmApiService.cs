using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImportShopApi.Services {
  public class TmApiService {
    public async Task<bool> IsValidToken(string token) {
      var result = await GetMe(token);

      return result != null;
    }

    public async Task<User> GetMe(string token) {
      try {
        return await new TelegramBotClient(token).GetMeAsync();
      }
      catch {
        return null;
      }
    }
  }
}