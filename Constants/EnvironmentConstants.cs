using System;
using System.Text;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.IdentityModel.Tokens;

namespace BotShopApi.Constants {
  public static class EnvironmentConstants {
    private static string Get(string name) => Environment.GetEnvironmentVariable(name);
    public static string Port => Get("PORT");
    private static string AwsAccessKeyId => Get("AWS_ACCESS_KEY_ID");
    private static string AwsSecretAccessKey => Get("AWS_SECRET_ACCESS_KEY");
    private static string AwsRegion => Get("AWS_REGION");
    public static string AwsBucketName => Get("AWS_BUCKET_NAME");
    public static string DataBaseUrl => Get("CLEARDB_DATABASE_URL");
    public static string AuthIssuer => Get("AUTH_ISSUER");
    public static string AuthAudience => Get("AUTH_AUDIENCE");
    private static string AuthKey => Get("AUTH_KEY");
    public static string AuthLifetimeInHours => Get("AUTH_LIFETIME_IN_HOURS");

    public static AWSOptions AwsCredentials => new AWSOptions {
      Credentials = new BasicAWSCredentials(AwsAccessKeyId, AwsSecretAccessKey),
      Region = RegionEndpoint.GetBySystemName(AwsRegion)
    };

    public static TokenValidationParameters TokenValidationParameters => new TokenValidationParameters {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = AuthIssuer,
      ValidAudience = AuthAudience,
      IssuerSigningKey = AuthKey.ToSymmetricSecurityKey()
    };

    public static SigningCredentials SigningCredentials => new SigningCredentials(
      AuthKey.ToSymmetricSecurityKey(),
      SecurityAlgorithms.HmacSha256
    );

    private static SymmetricSecurityKey ToSymmetricSecurityKey(this string key)
      => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
  }
}
