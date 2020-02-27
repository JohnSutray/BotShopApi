using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ImportShopCore.Extensions;
using Newtonsoft.Json;

namespace ImportShopApi.Extensions.Common {
  public static class ObjectExtensions {
    public static string ToJson(this object value, bool prettify = false) =>
      JsonConvert.SerializeObject(value, new JsonSerializerSettings {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = prettify ? Formatting.Indented : Formatting.None
      });

    public static object ToDto(
      this object value,
      IEnumerable<string> excludeProperties
    ) {
      var properties = value.GetType().GetProperties();

      var dto = new ExpandoObject() as IDictionary<string, object>;

      properties.ToList().ForEach(info => {
        if (excludeProperties.Contains(info.Name)) {
          return;
        }

        dto[info.Name.ToSnakeCase()] = info.GetValue(value);
      });

      return dto;
    }

    public static object ToDto(this object value, string excludeProperty) =>
      value.ToDto(excludeProperty.WrapIntoEnumerable());

    private static string ToSnakeCase(this string str) => str.First().ToString().ToLower() + str.Substring(1);
  }
}