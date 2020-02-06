using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ImportShopApi.Models.Account;
using ImportShopApi.Extensions.Configuration;
using Microsoft.Extensions.Configuration;

namespace ImportShopApi.Extensions
{
  public static class AccountExtensions
  {
    public static IEnumerable<Claim> GetAccountClaims(this Account account)
      => new[] {new Claim(ClaimsIdentity.DefaultNameClaimType, account.Id.ToString())};

    private static ClaimsIdentity GetAccountClaimsIdentity(this Account account)
      => new ClaimsIdentity(
        GetAccountClaims(account),
        "ApplicationCookie",
        ClaimsIdentity.DefaultNameClaimType,
        ClaimsIdentity.DefaultRoleClaimType
      );

    public static ClaimsPrincipal GetAccountClaimsPrincipal(this Account account)
      => new ClaimsPrincipal(GetAccountClaimsIdentity(account));

    public static string GetJwt(this Account account, IConfiguration configuration) =>
      new JwtSecurityTokenHandler().WriteToken(
        new JwtSecurityToken(
          issuer: configuration.GetTokenIssuer(),
          audience: configuration.GetTokenAudience(),
          notBefore: DateTime.UtcNow,
          claims: account.GetAccountClaims(),
          expires: configuration.GetTokenExpireTimeFromNow(),
          signingCredentials: configuration.GetSigningCredentials()
        )
      );
  }
}