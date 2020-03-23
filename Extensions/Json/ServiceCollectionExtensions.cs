using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ImportShopApi.Extensions.Json {
  public static class ServiceCollectionExtensions {
    public static IServiceCollection AddJsonServices(this IMvcBuilder builder)
      => builder.AddNewtonsoftJson(ConfigureJson).Services;
    
    private static void ConfigureJson(MvcNewtonsoftJsonOptions options) {
      options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }
  }
}