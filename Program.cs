using BotShopApi.Constants;
using BotShopCore.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BotShopApi {
  public class Program {
    public static void Main(string[] args) {
      DotEnvUtils.InjectDotEnvVars();
      CreateHostBuilder(args).Build().Run();
    }

    private static void ConfigureWebHost(IWebHostBuilder builder) => builder
      .UseStartup<Startup>()
      .UseUrls($"http://*:{EnvironmentConstants.Port}");

    private static IHostBuilder CreateHostBuilder(string[] args) => Host
      .CreateDefaultBuilder(args)
      .ConfigureWebHostDefaults(ConfigureWebHost);
  }
}
