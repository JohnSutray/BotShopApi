using ImportShopApi.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace ImportShopApi.Extensions.Swagger {
  public static class ServicesCollectionExtensions {
    public static IServiceCollection AddSwaggerServices(
      this IServiceCollection services
    ) => services.AddSwaggerGen(options => {
      options.SwaggerDoc(SwaggerVariables.Version, SwaggerVariables.OpenApiInfo);
      options.EnableAnnotations();
    });
  }
}