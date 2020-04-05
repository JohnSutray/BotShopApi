using Amazon.S3;
using ImportShopApi.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImportShopApi.Extensions.ServiceCollection {
  public static partial class ServiceCollectionExtensions {
    public static IServiceCollection AddAwsS3Services(
      this IServiceCollection services,
      IConfiguration configuration
    ) => services
      .AddAWSService<IAmazonS3>()
      .AddDefaultAWSOptions(configuration.GetAwsOptionsFromAppSettings());
  }
}