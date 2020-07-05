using BotShopApi.Extensions.ApplicationBuilder;
using BotShopApi.Extensions.ServiceCollection;
using BotShopCore;
using BotShopCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace BotShopApi {
  public class Startup {
    public void ConfigureServices(IServiceCollection services) => services
      .AddSwaggerServices()
      .AddAssemblyServices(typeof(Startup).Assembly)
      .AddAwsS3Services()
      .AddDbContext<ApplicationContext>()
      .AddControllers()
      .AddNewtonJsonServices()
      .AddCors()
      .AddJwtAuthentication();

    public void Configure(IApplicationBuilder builder) => builder
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
