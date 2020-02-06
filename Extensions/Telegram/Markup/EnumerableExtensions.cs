using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImportShopApi.Extensions.Telegram.Markup {
  public static class EnumerableExtensions {
    public static ReplyKeyboardMarkup ToKeyboardColumn(this IEnumerable<string> items)
      => new ReplyKeyboardMarkup(items.ToKeyboardButtons());

    private static IEnumerable<KeyboardButton> ToKeyboardButtons(this IEnumerable<string> items)
      => items.Select(item => item.ToKeyboardButton());
  }
}