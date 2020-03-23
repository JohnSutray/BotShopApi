using System;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;

namespace ImportShopApi.Constants {
  public static class SwaggerVariables {
    public const string Version = "v1";
    public const string ApiName = "Import Shop Api";

    public static string SwaggerJsonEndpoint => $"/swagger/{Version}/swagger.json";

    public static string PathToSwaggerXml => Path.Combine(AppContext.BaseDirectory, SwaggerXmlFileName);

    public static readonly OpenApiInfo OpenApiInfo = new OpenApiInfo { Title = ApiName, Version = Version };

    private static string SwaggerXmlFileName => $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  }
}