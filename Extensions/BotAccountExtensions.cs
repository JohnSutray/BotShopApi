using System.Collections.Generic;
using System.Security.Claims;
using ImportShopBot.Models;

namespace ImportShopBot.Extensions
{
    public static class BotAccountExtensions
    {
        private static IEnumerable<Claim> GetAccountClaims(this BotAccount botAccount)
            => new[] {new Claim(ClaimsIdentity.DefaultNameClaimType, botAccount.Id.ToString())};

        private static ClaimsIdentity GetAccountClaimsIdentity(this BotAccount botAccount)
            => new ClaimsIdentity(
                GetAccountClaims(botAccount),
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );

        public static ClaimsPrincipal GetAccountClaimsPrincipal(this BotAccount botAccount)
            => new ClaimsPrincipal(GetAccountClaimsIdentity(botAccount));
    }
}