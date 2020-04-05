using System.Linq;
using ImportShopApi.Constants;
using ImportShopApi.Extensions.Common;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace ImportShopApi.Extensions.Swagger {
  public static class ServicesCollectionExtensions {
    public static IServiceCollection AddSwaggerServices(
      this IServiceCollection services
    ) => services.AddSwaggerGen(options => {
      options.CustomOperationIds(GetOperationId);
      options.SwaggerDoc(SwaggerVariables.Version, SwaggerVariables.OpenApiInfo);
      options.EnableAnnotations();
    });

    private static string GetOperationId(ApiDescription operation) {
      var fullMethodName = operation.ActionDescriptor.DisplayName;
      var methodName = fullMethodName.Split(".").Last();
      var withoutDescription = methodName.Substring(0, methodName.IndexOf(" "));
      return withoutDescription.ToSnakeCase();
    }
  }
}