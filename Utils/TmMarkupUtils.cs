using System.Linq;
using ImportShopBot.Constants;
using ImportShopBot.Extensions.String;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImportShopBot.Utils
{
    public static class TmMarkupUtils
    {
        private static readonly KeyboardButton CatalogButton = TmLabelsConstants.Catalog.ToKeyboardButton();
        private static readonly KeyboardButton CartButton = TmLabelsConstants.Cart.ToKeyboardButton();
        private static readonly KeyboardButton CheckoutButton = TmLabelsConstants.Checkout.ToKeyboardButton();
        private static readonly KeyboardButton FeedbackButton = TmLabelsConstants.Feedback.ToKeyboardButton();

        public static readonly ReplyKeyboardMarkup MainMenuKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] {CatalogButton},
                new[] {CartButton},
                new[] {CheckoutButton},
                new[] {FeedbackButton}
            }
        );

        public static ReplyKeyboardMarkup CreateProductPageMenuKeyboard(int currentPage) => new ReplyKeyboardMarkup(
            new[]
            {
                new[]
                {
                    $"{TmLabelsConstants.PreviousPage} ({currentPage - 1})".ToKeyboardButton(),
                    $"{TmLabelsConstants.NextPage} ({currentPage + 1})".ToKeyboardButton()
                }
            }.Concat(MainMenuKeyboard.Keyboard)
        );
    }
}