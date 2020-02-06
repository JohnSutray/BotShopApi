using System;
using System.Linq;
using System.Threading.Tasks;
using ImportShopApi.Attributes;
using ImportShopApi.Constants;
using ImportShopApi.Enums;
using ImportShopApi.Extensions;
using ImportShopApi.Extensions.Enumerable;
using ImportShopApi.Extensions.TelegramContext;
using ImportShopApi.Models.Telegram;

namespace ImportShopApi.Controllers.Telegram
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
            var user = await context.GetUserAsync();

            await context.TelegramBotClient.SendTextWithMarkupAsync(
                context.GetUserTmId(),
                TmLabelsConstants.ChooseCategory,
                categoryList
            );

            user.ChatState = EChatState.CategoryList;
            await context.UserService.SaveChangesAsync();

            return true;
        }

        [TmMessageHandler]
        public async Task<bool> TypeList(TmMessageContext context)
        {
            var user = await context.GetUserAsync();

            if (user.ChatState != EChatState.CategoryList || !await context.MessageIsCategory())
                return false;

            var productTypesList = context.ProductService
                .Products
                .Where(p => p.Category == user.LastSelectedCategory)
                .AsEnumerable()
                .GetGroups(p => p.Type)
                .ToKeyboardColumn();

            await context.TelegramBotClient.SendTextWithMarkupAsync(
                context.GetUserTmId(),
                TmLabelsConstants.ChooseType,
                productTypesList
            );

            user.LastSelectedCategory = context.Message.Text;
            user.ChatState = EChatState.TypeList;

            await context.UserService.SaveChangesAsync();

            return true;
        }

        [TmMessageHandler]
        public async Task<bool> ToProductList(TmMessageContext context)
        {
            var user = await context.GetUserAsync();

            if (user.ChatState != EChatState.TypeList || !await context.MessageIsProductType())
                return false;

            user.LastSelectedType = context.Message.Text;
            user.ChatState = EChatState.ProductList;

            await context.PaginateProductList(0);
            await context.UserService.SaveChangesAsync();

            return true;
        }

        [TmMessageHandler(
            "("
            + TmLabelsConstants.PreviousPage
            + "|"
            + TmLabelsConstants.NextPage
            + ")"
            + " " +
            "\\(" 
            + "\\d+" 
            + " "
            + TmLabelsConstants.Page
            + "\\)"
        )]
        public async Task<bool> PaginateProducts(TmMessageContext context)
        {
            var user = await context.GetUserAsync();

            Console.WriteLine(context.Message.Text);

            if (user.ChatState != EChatState.ProductList)
                return false;

            await context.PaginateProductList(context.Message.Text.ParsePageFromMessage());

            return true;
        }
    }
}