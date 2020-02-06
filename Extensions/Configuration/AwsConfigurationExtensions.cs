using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;

namespace ImportShopApi.Extensions.Configuration {
  public static partial class ConfigurationExtensions {
    private static IConfigurationSection GetAwsSection(this IConfiguration configuration)
      => configuration.GetSectionByName("AWS");

    private static AWSCredentials GetAwsCredentials(this IConfiguration configuration) => new BasicAWSCredentials(
      configuration.GetAwsSection()["AccessKeyId"],
      configuration.GetAwsSection()["SecretAccessKey"]
    );

    private static RegionEndpoint GetRegion(this IConfiguration configuration) => RegionEndpoint.GetBySystemName(
      configuration.GetAwsSection()["Region"]
    );

    public static AWSOptions GetAwsOptionsFromAppSettings(this IConfiguration configuration) => new AWSOptions {
      Credentials = configuration.GetAwsCredentials(),
      Region = configuration.GetRegion()
    };
  }
}