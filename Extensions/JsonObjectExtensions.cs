using Newtonsoft.Json;

namespace ImportShopBot.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object value) => JsonConvert.SerializeObject(value);
    }
}