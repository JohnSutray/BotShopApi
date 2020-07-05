using System.Linq;
using BotShopApi.Constants;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BotShopApi.Extensions.ServiceCollection {
  public static partial class ServiceCollectionExtensions {
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services) =>
      services.AddSwaggerGen(ConfigureSwaggerGenerator);

    private static void ConfigureSwaggerGenerator(SwaggerGenOptions options) {
      options.CustomOperationIds(GetOperationId);
      options.SwaggerDoc(SwaggerVariables.Version, SwaggerVariables.OpenApiInfo);
      options.EnableAnnotations();
    }

    private static string GetOperationId(ApiDescription operation) =>
      operation.ActionDescriptor.DisplayName.GetMethodName().WithoutDescriptionPart().ToSnakeCase();

    private static string GetMethodName(this string fullPathName) => fullPathName.Split(".").Last();

    private static string WithoutDescriptionPart(this string str) => str.Substring(0, str.IndexOf(" "));

    private static string ToSnakeCase(this string str) => str.First().ToString().ToLower() + str.Substring(1);
  }
}