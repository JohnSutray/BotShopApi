using Newtonsoft.Json;

namespace ImportShopApi.Extensions.Common {
  public static class ObjectExtensions {
    public static string ToJson(this object value) => JsonConvert.SerializeObject(value);
  }
}