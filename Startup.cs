using ImportShopApi.Extensions;
using ImportShopApi.Extensions.Authentication;
using ImportShopApi.Extensions.Aws;
using ImportShopApi.Extensions.Json;
using ImportShopApi.Extensions.Swagger;
using ImportShopCore;
using ImportShopCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImportShopApi {
  public class Startup {
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services) => services
      .AddSwaggerServices()
      .AddAssemblyServices(typeof(Startup).Assembly)
      .AddAwsS3(Configuration)
      .AddDbContext<ApplicationContext>()
      .AddControllers()
      .AddJsonServices()
      .AddCors()
      .AddJwtAuthentication(Configuration);

    public void Configure(IApplicationBuilder application, IWebHostEnvironment environment) {
      if (environment.IsProduction()) application.UseHsts();

      application
        .UseSwaggerUIAndApi()
        .UseCorsAllowingAnyRequest()
        .UseJsonExceptionHandler()
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseHttpsRedirection()
        .UseForwardedHeaders(new ForwardedHeadersOptions {
          ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        })
        .UseEndpoints(e => e.MapControllers());
    }
  }
}