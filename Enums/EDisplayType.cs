using System.Text.Json.Serialization;

namespace ImportShopApi.Enums
{
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum EDisplayType
  {
    Video,
    Image,
  }
}