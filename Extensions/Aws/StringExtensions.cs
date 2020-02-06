using System;
using ImportShopApi.Enums;
using Microsoft.AspNetCore.StaticFiles;

namespace ImportShopApi.Extensions.Aws {
  public static partial class StringExtensions {
    private static FileExtensionContentTypeProvider ExtensionContentTypeProvider { get; }
      = new FileExtensionContentTypeProvider();

    public static string GetContentType(this string fileName) {
      ExtensionContentTypeProvider.TryGetContentType(fileName, out var contentType);

      return contentType;
    }

    public static EDisplayType GetDisplayType(this string fileName) {
      var contentType = GetContentType(fileName);

      if (contentType.StartsWith("image")) return EDisplayType.Image;
      if (contentType.StartsWith("video")) return EDisplayType.Video;

      throw new Exception("Incorrect file");
    }
    
    public static string UrlWithoutQueryParams(this string url) => url.Substring(
      0, url.IndexOf("?", StringComparison.Ordinal)
    );

    public static string GetS3Key(this string url) => new Uri(url).AbsolutePath.Substring(1);
  }
}