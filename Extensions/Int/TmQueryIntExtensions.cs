using ImportShopApi.Constants;

namespace ImportShopApi.Extensions.Int
{
    public static class IntExtensions
    {
        public static string ToAddProductQuery(this int productId) => $"{QueryConstants.Cart}/{QueryConstants.Add}/{productId}";
    }
}