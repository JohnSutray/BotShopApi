using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BotShopApi.Constants;
using BotShopCore.Attributes;
using BotShopCore.Extensions;

namespace BotShopApi.Services {
  [Service]
  public class JwtService {
    private DateTime JwtExpiredAt => DateTime.UtcNow.Add(
      TimeSpan.FromHours(EnvironmentConstants.AuthLifetimeInHours.ParseInt())
    );

    public string CreateJwt(int accountId) => new JwtSecurityTokenHandler().WriteToken(
      new JwtSecurityToken(
        issuer: EnvironmentConstants.AuthIssuer,
        audience: EnvironmentConstants.AuthAudience,
        notBefore: DateTime.UtcNow,
        claims: CreateAccountClaims(accountId),
        expires: JwtExpiredAt,
        signingCredentials: EnvironmentConstants.SigningCredentials
      )
    );

    private static IEnumerable<Claim> CreateAccountClaims(int accountId) => new Claim(
      ClaimsIdentity.DefaultNameClaimType,
      accountId.ToString()
    ).WrapIntoEnumerable();
  }
}
