using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImportShopBot.Extensions.Enumerable;
using ImportShopBot.Extensions.String;
using ImportShopBot.Models.Product;
using ImportShopBot.Models.Telegram;
using ImportShopBot.Utils;

namespace ImportShopBot.Extensions
{
    public static class TmMessageContextExtensions
    {
        private const string PageGroup = "pageGroup";
        private const int ProductPageLimit = 10;
        private static readonly Regex ProductPageParseRegex = new Regex($"\\((?<{PageGroup}>\\d+)\\)");

        public static async Task PaginateProductList(this TmMessageContext context, int page) =>
            await Task.WhenAll(
                    new List<Product>()
                // context.ProductService.Products
                // .PaginateAndFilter(
                //     page,
                //     ProductPageLimit,
                //     FilterByCategoryAndType(context.User.LastSelectedCategory, context.User.LastSelectedType)
                // )
                .Select(context.SendProduct)
            );

        private static Task SendProduct(this TmMessageContext context, Product product) =>
            context.TelegramBotClient.SendMediaWithMarkupAsync(
                context.Message.From.Id,
                product.MediaUrl.ToInputMedia(),
                product.ToProductCaption(),
                TmMarkupUtils.CreateProductPageMenuKeyboard(context.Message.Text.ParsePageFromMessage())
            );

        public static int ParsePageFromMessage(this string message) => ProductPageParseRegex
            .Match(message).Groups[PageGroup].Value.ParseInt();

        private static Func<Product, bool> FilterByCategoryAndType(string category, string type) =>
            product => product.Category == category && product.Type == type;


    }
}