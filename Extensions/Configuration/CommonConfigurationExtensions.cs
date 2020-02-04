using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ImportShopBot.Extensions.Configuration
{
    public static partial class ConfigurationExtensions
    {
        private static IConfigurationSection GetSectionByName(this IConfiguration configuration, string name) => 
            configuration.GetChildren().First(child => child.Key == name);
    }
}