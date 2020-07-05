using Amazon.S3;
using BotShopApi.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace BotShopApi.Extensions.ServiceCollection {
  public static partial class ServiceCollectionExtensions {
    public static IServiceCollection AddAwsS3Services(this IServiceCollection services) => services
      .AddAWSService<IAmazonS3>()
      .AddDefaultAWSOptions(EnvironmentConstants.AwsCredentials);
  }
}
