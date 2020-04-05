using ImportShopApi.Constants;
using Microsoft.AspNetCore.Builder;

namespace ImportShopApi.Extensions.ApplicationBuilder {
  public static partial class ApplicationBuilderExtensions {
    public static IApplicationBuilder UseSwaggerOption(
      this IApplicationBuilder application
    ) => application
      .UseSwagger()
      .UseSwaggerUI(options => options.SwaggerEndpoint(
        SwaggerVariables.SwaggerJsonEndpoint,
        SwaggerVariables.ApiName
      ));
  }
}