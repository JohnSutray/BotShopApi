using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ImportShopApi.Extensions.Configuration;
using ImportShopCore.Extensions;
using Microsoft.Extensions.Configuration;

namespace ImportShopApi.Extensions {
  public static class IntExtensions {
    public static string CreateJwt(
      this int accountId,
      IConfiguration configuration
    ) => new JwtSecurityTokenHandler().WriteToken(
      new JwtSecurityToken(
        issuer: configuration.GetTokenIssuer(),
        audience: configuration.GetTokenAudience(),
        notBefore: DateTime.UtcNow,
        claims: accountId.CreateAccountClaims(),
        expires: configuration.GetTokenExpireTimeFromNow(),
        signingCredentials: configuration.GetSigningCredentials()
      )
    );

    private static IEnumerable<Claim> CreateAccountClaims(this int accountId) =>
      new Claim(
        ClaimsIdentity.DefaultNameClaimType,
        accountId.ToString()
      ).WrapIntoEnumerable();
  }
}