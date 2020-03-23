using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace ImportShopApi.Extensions.Authentication {
  public static class ApplicationBuilderExtensions {
    private static void ConfigureCors(CorsPolicyBuilder builder) => builder
      .AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod();

    public static IApplicationBuilder UseCorsAllowingAnyRequest(this IApplicationBuilder application)
      => application.UseCors(ConfigureCors);
  }
}