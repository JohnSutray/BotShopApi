using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ImportShopBot.Extensions.String
{
    public static partial class StringExtensions
    {
        public static SymmetricSecurityKey ToSymmetricSecurityKey(this string key)
        => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
    }
}