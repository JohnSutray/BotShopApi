using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImportShopApi.Extensions.Aws {
  public static class ServiceCollectionExtensions {
    public static IServiceCollection AddAwsS3(
      this IServiceCollection services,
      IConfiguration configuration
    ) => services
      .AddAWSService<IAmazonS3>()
      .AddDefaultAWSOptions(configuration.GetAwsOptionsFromAppSettings());
  }
}