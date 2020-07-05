using Microsoft.OpenApi.Models;

namespace BotShopApi.Constants {
  public static class SwaggerVariables {
    public const string Version = "v1";
    public const string ApiName = "Bot Shop Api";

    public static string SwaggerJsonEndpoint => $"/swagger/{Version}/swagger.json";

    public static readonly OpenApiInfo OpenApiInfo = new OpenApiInfo {Title = ApiName, Version = Version};
  }
}
