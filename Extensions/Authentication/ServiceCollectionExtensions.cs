using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImportShopApi.Extensions.Authentication {
  public static class ServiceCollectionExtensions {
    public static void AddJwtAuthentication(
      this IServiceCollection services,
      IConfiguration configuration
    ) => services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
      options.RequireHttpsMetadata = false;
      options.TokenValidationParameters = configuration.GetTokenValidationParameters();
    });
  }
}