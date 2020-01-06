using System.Threading.Tasks;
using ImportShopBot.Extensions;
using ImportShopBot.Models;
using ImportShopBot.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImportShopBot.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : Controller
    {
        private AccountService AccountService { get; }

        public AuthenticateController(AccountService accountService) => AccountService = accountService;

        [HttpPost]
        [Route("sign-in")]
        public async Task<IActionResult> SignIn(BotAccountLogin accountLogin)
        {
            if (!ModelState.IsValid)
                return this.UnprocessableModel();

            if (!(await AccountService.FindByToken(accountLogin.TelegramToken) is { } botAccount))
                return this.AddModelError("Некорректный Telegram токен или аккаунт не существует.")
                    .UnprocessableModel();


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, botAccount.GetAccountClaimsPrincipal());

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }
    }
}