using ImportShopApi.Extensions.ApplicationBuilder;
using ImportShopApi.Extensions.ServiceCollection;
using ImportShopCore;
using ImportShopCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImportShopApi {
  public class Startup {
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services) => services
      .AddSwaggerServices()
      .AddAssemblyServices(typeof(Startup).Assembly)
      .AddAwsS3Services(Configuration)
      .AddDbContext<ApplicationContext>()
      .AddControllers()
      .AddNewtonJsonServices()
      .AddCors()
      .AddJwtAuthentication(Configuration);

    public void Configure(IApplicationBuilder application) => application
      .UseForwardedHeaders(new ForwardedHeadersOptions {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      })
      .UseSwaggerOption()
      .UseCorsOption()
      .UseJsonExceptionHandlerOption()
      .UseRouting()
      .UseAuthentication()
      .UseAuthorization()
      .UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());
  }
}