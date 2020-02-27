using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ImportShopApi.Extensions.Authentication {
  public static class StringExtensions {
    public static SymmetricSecurityKey ToSymmetricSecurityKey(this string key)
      => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
  }
}