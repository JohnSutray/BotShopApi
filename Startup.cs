using System;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using ImportShopBot.Contexts;
using ImportShopBot.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImportShopBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        private IConfiguration Configuration { get; }
        private string ConnectionString => Configuration.GetConnectionString("DefaultConnection");

        private BasicAWSCredentials AwsCredentials => new BasicAWSCredentials(
            Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
            Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")
        );

        private AWSOptions AwsOptions => new AWSOptions
        {
            Region = RegionEndpoint.EUNorth1,
            Credentials = AwsCredentials
        };

        private void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(ConnectionString);

        private void ConfigureCookies(CookieAuthenticationOptions options)
            => options.LoginPath = new PathString("/auth/sign-in");

        private void ConfigureDatabaseContexts(IServiceCollection services) => services
            .AddDbContext<ProductContext>(ConfigureDbContext)
            .AddDbContext<BotAccountContext>(ConfigureDbContext);

        private void ConfigureApplication(IServiceCollection services) => services
            .AddControllers()
            .Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(ConfigureCookies);

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatabaseContexts(services);
            ConfigureApplication(services);
            ConfigureCustomServices(services);
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddTransient<ProductService>()
                .AddTransient<AccountService>()
                .AddAWSService<IAmazonS3>()
                .AddTransient<MediaStorageService>()
                .AddDefaultAWSOptions(AwsOptions);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(e => e.MapControllers());
        }
    }
}