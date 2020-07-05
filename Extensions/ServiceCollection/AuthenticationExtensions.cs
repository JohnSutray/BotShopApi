using BotShopApi.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace BotShopApi.Extensions.ServiceCollection {
  public static partial class ServiceCollectionExtensions {
    public static void AddJwtAuthentication(this IServiceCollection services) =>
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = EnvironmentConstants.TokenValidationParameters;
      });
  }
}
