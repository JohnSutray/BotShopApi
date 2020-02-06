using System;

namespace ImportShopApi.Extensions.String
{
    public static partial class StringExtensions
    {
        public static string UrlWithoutQueryParams(this string url) => url.Substring(
            0, url.IndexOf("?", StringComparison.Ordinal)
        );

        public static string GetS3Key(this string url) => new Uri(url).AbsolutePath.Substring(1);
    }
}