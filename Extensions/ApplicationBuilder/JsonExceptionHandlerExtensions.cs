using System.Net;
using System.Threading.Tasks;
using ImportShopCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ImportShopApi.Extensions.ApplicationBuilder {
  public static partial class ApplicationBuilderExtensions {
    public static IApplicationBuilder UseJsonExceptionHandlerOption(this IApplicationBuilder app) => app
      .UseExceptionHandler(applicationBuilder => applicationBuilder.Run(SerializeError));

    private static async Task SerializeError(this HttpContext context) {
      context.SetErrorStatusCode();
      context.SetJsonContentType();

      if (context.GetErrorMessage() is {} errorMessage)
        await context.WriteMessage(errorMessage);
    }

    private static void SetErrorStatusCode(this HttpContext context) =>
      context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

    private static void SetJsonContentType(this HttpContext context) =>
      context.Response.ContentType = "application/json";

    private static string GetErrorMessage(this HttpContext context) =>
      context.Features.Get<IExceptionHandlerFeature>()?.Error.Message;

    private static async Task WriteMessage(this HttpContext context, string message) =>
      await context.Response.WriteAsync(
        message.WrapIntoEnumerable().ToJson()
      );

    private static string ToJson(this object value) => JsonConvert.SerializeObject(
      value,
      new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}
    );
  }
}