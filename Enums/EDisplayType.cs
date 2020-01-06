using System.Text.Json.Serialization;

namespace ImportShopBot.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EDisplayType
    {
        Video,
        Image,
    }
}