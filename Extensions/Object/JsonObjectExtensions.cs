using Newtonsoft.Json;

namespace ImportShopApi.Extensions.Object
{
  public static class ObjectExtensions
  {
    public static string ToJson(this object value) => JsonConvert.SerializeObject(value);
  }
}