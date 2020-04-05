using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ImportShopApi {
  public class Program {
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
      .ConfigureWebHostDefaults(webBuilder => {
        webBuilder.ConfigureKestrel(options => options.Listen(
          IPAddress.Loopback,
          443,
          listenOptions => listenOptions.UseHttps()
        ));
        webBuilder.UseStartup<Startup>();
      });
  }
}