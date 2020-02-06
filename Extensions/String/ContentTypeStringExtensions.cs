using System;
using ImportShopApi.Enums;
using Microsoft.AspNetCore.StaticFiles;

namespace ImportShopApi.Extensions.String {
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
  }
}