using System;
using System.IO;

namespace ImportShopApi.Extensions.Common {
  public static class MemoryStreamExtensions {
    public static string ToBase64(this MemoryStream stream, string dataTypePrefix) {
      return dataTypePrefix + Convert.ToBase64String(stream.ToArray());
    }
  }
}