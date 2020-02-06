using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImportShopApi.Constants;
using ImportShopApi.Extensions.Common;
using ImportShopApi.Extensions.Telegram.Markup;
using ImportShopApi.Models;
using ImportShopApi.Models.Product;
using ImportShopApi.Models.Telegram;
using Microsoft.EntityFrameworkCore;

namespace ImportShopApi.Extensions.Telegram {
  public static class TmMessageContextExtensions {
    private const string PageGroup = "pageGroup";
    private const int ProductPageLimit = 3;

    private static readonly Regex ProductPageParseRegex =
      new Regex($"\\((?<{PageGroup}>\\d+) {TmLabelsConstants.Page}\\)");

    public static int GetUserTmId(this TmMessageContext context) => context.Message.From.Id;

    public static async Task<TmUser> GetUserAsync(this TmMessageContext context) =>
      await context.UserService.GetUserByTmId(context.GetUserTmId());

    public static async Task PaginateProductList(this TmMessageContext context, int page) {
      var user = await context.GetUserAsync();
      var productPage = context.ProductService.Products
        .Where(product => product.Category == user.LastSelectedCategory && product.Type == user.LastSelectedType)
        .Paginate(page, ProductPageLimit);

      await Task.WhenAll(productPage.Items.Select(context.SendProduct));
      await context.SendSelectProductMessage(productPage);
    }

    private static async Task SendSelectProductMessage(
      this TmMessageContext context,
      PaginateResult<Product> productPage
    ) => await context.TelegramBotClient.SendTextWithMarkupAsync(
      context.GetUserTmId(),
      TmLabelsConstants.AddProductsToCart,
      (productPage.Page + 1).ToProductPageMenuKeyboard(
        !productPage.IsFirstPage,
        !productPage.IsLastPage
      )
    );

    private static Task SendProduct(this TmMessageContext context, Product product) => context.TelegramBotClient
      .SendMediaWithMarkupAsync(
        context.GetUserTmId(),
        product.MediaUrl.ToInputMedia(),
        product.ToProductCaption(),
        product.Id.ToAddToCartButtonMarkup()
      );

    public static int ParsePageFromMessage(this string message) =>
      ProductPageParseRegex.Match(message).Groups[PageGroup].Value
        .ParseInt() - 1;

    public static async Task<bool> MessageIsCategory(this TmMessageContext context) =>
      await context.ProductService.Products.AnyAsync(p => p.Category == context.Message.Text);

    public static async Task<bool> MessageIsProductType(this TmMessageContext context) =>
      await context.ProductService.Products.AnyAsync(p => p.Type == context.Message.Text);
  }
}