using System;

namespace ImportShopApi.Extensions.Aws {
  public static class StringExtensions {
    public static string UrlWithoutQueryParams(this string url) => url.Substring(
      0, url.IndexOf("?", StringComparison.Ordinal)
    );

    public static string GetS3Key(this string url) => new Uri(url).AbsolutePath.Substring(1);
  }
}