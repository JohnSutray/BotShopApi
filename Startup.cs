using System;
using System.Text.RegularExpressions;
using Amazon.S3;
using ImportShopApi.Constants;
using ImportShopApi.Contexts;
using ImportShopApi.Services;
using ImportShopApi.Extensions;
using ImportShopApi.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImportShopApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration) => Configuration = configuration;

    private IConfiguration Configuration { get; }
    private string ConnectionString => Configuration.GetConnectionString("DefaultConnection");

    private void ConfigureCors(CorsPolicyBuilder builder) => builder
      .AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod();

    private void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder.UseMySql(ConnectionString);

    private void ConfigureJwt(JwtBearerOptions options)
    {
      options.RequireHttpsMetadata = false;
      options.TokenValidationParameters = Configuration.GetTokenValidationParameters();
    }

    private void ConfigureContexts(IServiceCollection services) => services
      .AddDbContext<ProductContext>(ConfigureDbContext)
      .AddDbContext<AccountContext>(ConfigureDbContext)
      .AddDbContext<TmUserContext>(ConfigureDbContext);

    private void ConfigureAspServices(IServiceCollection services) => services
      .AddControllers()
      .Services
      .AddCors()
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(ConfigureJwt);

    public void ConfigureServices(IServiceCollection services)
    {
      ConfigureContexts(services);
      ConfigureAspServices(services);
      ConfigureCustomServices(services);
      ConfigureTelegramServices(services);
    }

    private void ConfigureCustomServices(IServiceCollection services) => services
      .AddTransient<ProductService>()
      .AddTransient<AccountService>()
      .AddTransient<MediaStorageService>()
      .AddAWSService<IAmazonS3>()
      .AddDefaultAWSOptions(Configuration.GetAwsOptionsFromAppSettings());

    private void ConfigureTelegramServices(IServiceCollection services) => services
      .AddTransient<TmProductService>()
      .AddTransient<TmUserService>()
      .AddTransient<TmAccountService>()
      .AddTransient<TmApiService>()
      .AddSingleton<TmBotManagerService>();

    public void Configure(IApplicationBuilder app, IServiceProvider services)
    {
      app.UseCors(ConfigureCors)
        .UseDeveloperExceptionPage()
        .UseJsonExceptionHandler()
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseEndpoints(e => e.MapControllers());

      services.GetRequiredService<TmBotManagerService>().UpdateBots();
    }
  }
}