using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ImportShopApi {
  public class Program {
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

    private static void ConfigureWebHost(IWebHostBuilder builder) => builder
      .UseStartup<Startup>()
      .UseUrls("https://localhost:5000");

    private static IHostBuilder CreateHostBuilder(string[] args) => Host
      .CreateDefaultBuilder(args)
      .ConfigureWebHostDefaults(ConfigureWebHost);
  }
}