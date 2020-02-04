using System;
using System.Linq;
using System.Threading.Tasks;
using ImportShopBot.Attributes;
using ImportShopBot.Constants;
using ImportShopBot.Enums;
using ImportShopBot.Extensions;
using ImportShopBot.Extensions.Enumerable;
using ImportShopBot.Models.Product;
using ImportShopBot.Models.Telegram;

namespace ImportShopBot.Controllers.Telegram
{
    public class TelegramProductController
    {
        [TmMessageHandler(TmLabelsConstants.Catalog)]
        public async Task<bool> CategoryList(TmMessageContext context)
        {
            var categoryList = context.ProductService
                .Products
                .AsEnumerable()
                .GetGroups(p => p.Category)
                .ToKeyboardColumn();

            await context.TelegramBotClient.SendTextWithMarkupAsync(
                context.Message.From.Id,
                TmLabelsConstants.ChooseCategory,
                categoryList
            );

            context.User.ChatState = EChatState.CategoryList;

            return true;
        }

        [TmMessageHandler]
        public async Task<bool> TypeList(TmMessageContext context)
        {
            if (context.User.ChatState != EChatState.CategoryList || !MessageIsCategory(context))
                return false;

            bool Filter(Product p) => p.Category == context.User.LastSelectedCategory;
            string GroupBy(Product p) => p.Type;

            var productTypesList = context.ProductService
                .Products
                .AsEnumerable()
                .Where(Filter)
                .GetGroups(GroupBy)
                .ToKeyboardColumn();

            await context.TelegramBotClient.SendTextWithMarkupAsync(
                context.Message.From.Id,
                TmLabelsConstants.ChooseType,
                productTypesList
            );

            var a = productTypesList.Keyboard.First().ToList();

            context.User.LastSelectedCategory = context.Message.Text;
            context.User.ChatState = EChatState.TypeList;

            return true;
        }

        [TmMessageHandler]
        public async Task<bool> ToProductList(TmMessageContext context)
        {
            if (context.User.ChatState != EChatState.TypeList || !MessageIsProductType(context))
                return false;

            context.User.LastSelectedType = context.Message.Text;
            context.User.ChatState = EChatState.ProductList;
            
            await context.PaginateProductList(0);

            return true;
        }

        [TmMessageHandler(
            "("
            + TmLabelsConstants.PreviousPage
            + "|"
            + TmLabelsConstants.NextPage
            + ")"
            + " \\(\\d+\\)"
        )]
        public async Task<bool> PaginateProducts(TmMessageContext context)
        {
            if (context.User.ChatState != EChatState.ProductList)
                return false;

            await context.PaginateProductList(context.Message.Text.ParsePageFromMessage());

            return true;
        }

        private bool MessageIsCategory(TmMessageContext context) =>
            context.ProductService.Products.Any(p => p.Category == context.Message.Text);

        private bool MessageIsProductType(TmMessageContext context) =>
            context.ProductService.Products.Any(p => p.Type == context.Message.Text);
    }
}