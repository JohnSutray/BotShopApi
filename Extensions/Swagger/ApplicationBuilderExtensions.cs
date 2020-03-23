using ImportShopApi.Constants;
using Microsoft.AspNetCore.Builder;

namespace ImportShopApi.Extensions.Swagger {
  public static class ApplicationBuilderExtensions {
    public static IApplicationBuilder UseSwaggerUIAndApi(
      this IApplicationBuilder application
    ) => application
      .UseSwagger()
      .UseSwaggerUI(options => options.SwaggerEndpoint(
        SwaggerVariables.SwaggerJsonEndpoint,
        SwaggerVariables.ApiName
      ));
  }
}