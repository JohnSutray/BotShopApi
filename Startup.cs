using Amazon.S3;
using ImportShopApi.Extensions;
using ImportShopApi.Extensions.Authentication;
using ImportShopApi.Extensions.Aws;
using ImportShopApi.Services;
using ImportShopCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ImportShopApi {
  public class Startup {
    public Startup(IConfiguration configuration) => Configuration = configuration;

    private IConfiguration Configuration { get; }

    private void ConfigureCors(CorsPolicyBuilder builder) => builder
      .AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod();

    private void ConfigureJwt(JwtBearerOptions options) {
      options.RequireHttpsMetadata = false;
      options.TokenValidationParameters = Configuration.GetTokenValidationParameters();
    }

    private void ConfigureJson(MvcNewtonsoftJsonOptions options) {
      options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    private void ConfigureContexts(IServiceCollection services) => services.AddDbContext<ApplicationContext>();

    private void ConfigureAspServices(IServiceCollection services) => services
      .AddControllers()
      .AddNewtonsoftJson(ConfigureJson)
      .Services
      .AddCors()
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(ConfigureJwt);

    public void ConfigureServices(IServiceCollection services) {
      ConfigureContexts(services);
      ConfigureAspServices(services);
      ConfigureCustomServices(services);
    }

    private void ConfigureCustomServices(IServiceCollection services) => services
      .AddTransient<ProductService>()
      .AddTransient<AccountService>()
      .AddTransient<MediaStorageService>()
      .AddAWSService<IAmazonS3>()
      .AddDefaultAWSOptions(Configuration.GetAwsOptionsFromAppSettings());

    public void Configure(IApplicationBuilder application) => application
      .UseCors(ConfigureCors)
      .UseDeveloperExceptionPage()
      .UseJsonExceptionHandler()
      .UseRouting()
      .UseAuthentication()
      .UseAuthorization()
      .UseEndpoints(e => e.MapControllers());
  }
}