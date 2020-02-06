using ImportShopApi.Extensions;
using ImportShopApi.Extensions.String;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImportShopApi.Constants
{
  public static class TmMarkupConstants
  {
    private static readonly KeyboardButton CatalogButton = TmLabelsConstants.Catalog.ToKeyboardButton();
    private static readonly KeyboardButton CartButton = TmLabelsConstants.Cart.ToKeyboardButton();
    private static readonly KeyboardButton CheckoutButton = TmLabelsConstants.Checkout.ToKeyboardButton();
    private static readonly KeyboardButton FeedbackButton = TmLabelsConstants.Feedback.ToKeyboardButton();

    public static readonly ReplyKeyboardMarkup MainMenuKeyboard = new ReplyKeyboardMarkup(new[]
      {
        CatalogButton.WrapIntoEnumerable(),
        CartButton.WrapIntoEnumerable(),
        CheckoutButton.WrapIntoEnumerable(),
        FeedbackButton.WrapIntoEnumerable()
      }
    );
  }
}