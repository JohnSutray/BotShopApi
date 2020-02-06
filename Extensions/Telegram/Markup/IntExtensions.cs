using System.Collections.Generic;
using System.Linq;
using ImportShopApi.Constants;
using ImportShopApi.Extensions.Common;
using ImportShopApi.Extensions.Telegram.Int;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImportShopApi.Extensions.Telegram.Markup {
  public static class TmMarkupExtensions {
    public static ReplyKeyboardMarkup ToProductPageMenuKeyboard(
      this int currentPage,
      bool previousExists,
      bool nextExists
    ) {
      var paginationRow = new List<KeyboardButton>();

      if (previousExists) {
        var previousPageButton = TmLabelsConstants.PaginationLabel(currentPage - 1).ToKeyboardButton();
        paginationRow.Add(previousPageButton);
      }

      if (nextExists) {
        var nextPageButton = TmLabelsConstants.PaginationLabel(currentPage + 1).ToKeyboardButton();
        paginationRow.Add(nextPageButton);
      }

      var withMainMenu = paginationRow.WrapIntoEnumerable()
        .Concat(TmMarkupConstants.MainMenuKeyboard.Keyboard);

      return new ReplyKeyboardMarkup(withMainMenu);
    }

    public static InlineKeyboardMarkup ToAddToCartButtonMarkup(this int productId) =>
      new InlineKeyboardMarkup(
        TmLabelsConstants.AddToCart
          .ToInlineKeyboardButton(productId.ToAddProductQuery())
          .WrapIntoEnumerable()
      );
  }
}