using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ImportShopApi.Extensions.Common {
  public static class ConfigurationExtensions {
    public static IConfigurationSection GetSectionByName(this IConfiguration configuration, string name) =>
      configuration.GetChildren().First(child => child.Key == name);
  }
}