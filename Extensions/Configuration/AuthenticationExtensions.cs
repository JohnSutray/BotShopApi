using System;
using ImportShopApi.Extensions.String;
using ImportShopCore.Extensions;
using ImportShopCore.Extensions.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ImportShopApi.Extensions.Configuration {
  public static partial class ConfigurationExtensions {
    private static IConfigurationSection GetAuthorizeSection(this IConfiguration configuration) =>
      configuration.GetSectionByName("Authorization");

    public static string GetTokenIssuer(this IConfiguration configuration) =>
      configuration.GetAuthorizeSection()["Issuer"];

    public static string GetTokenAudience(this IConfiguration configuration) =>
      configuration.GetAuthorizeSection()["Audience"];

    private static SymmetricSecurityKey GetTokenKey(this IConfiguration configuration) =>
      configuration.GetAuthorizeSection()["Key"].ToSymmetricSecurityKey();

    private static int GetTokenLifeTimeInHours(this IConfiguration configuration) =>
      configuration.GetAuthorizeSection()["LifetimeInHours"].ParseInt();

    public static DateTime GetTokenExpireTimeFromNow(this IConfiguration configuration) =>
      DateTime.UtcNow.Add(
        TimeSpan.FromHours(
          configuration.GetTokenLifeTimeInHours()
        )
      );

    public static SigningCredentials GetSigningCredentials(this IConfiguration configuration) =>
      new SigningCredentials(
        configuration.GetTokenKey(),
        SecurityAlgorithms.HmacSha256
      );

    public static TokenValidationParameters GetTokenValidationParameters(this IConfiguration configuration) =>
      new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration.GetTokenIssuer(),
        ValidAudience = configuration.GetTokenAudience(),
        IssuerSigningKey = configuration.GetTokenKey()
      };
  }
}