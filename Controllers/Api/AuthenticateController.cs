using System.Threading.Tasks;
using ImportShopBot.Constants;
using ImportShopBot.Extensions;
using ImportShopBot.Models.Account;
using ImportShopBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImportShopBot.Controllers.Api
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : Controller
    {
        private AccountService AccountService { get; }
        private IConfiguration Configuration { get; }
        private TmApiService TmApiService { get; }

        public AuthenticateController(
            AccountService accountService,
            TmApiService tmApiService,
            IConfiguration configuration
        )
        {
            AccountService = accountService;
            Configuration = configuration;
            TmApiService = tmApiService;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(AccountLogin accountLogin)
        {
            if (!ModelState.IsValid) return this.UnprocessableModel();

            var account = await AccountService.FindByToken(accountLogin.TelegramToken);

            if (account == null)
                return this.AddModelError(MessageConstants.NoAccountWithCurrentToken)
                    .UnprocessableModel();
            
            var botInfo = await TmApiService.GetMe(account.TelegramToken);

            return Ok(new
            {
                Token = account.GetJwt(Configuration),
                Name = botInfo.Username
            });
        }
    }
}