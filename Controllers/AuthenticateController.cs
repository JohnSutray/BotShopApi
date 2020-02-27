using System.Threading.Tasks;
using ImportShopCore.Models.Account;
using ImportShopApi.Constants;
using ImportShopApi.Extensions;
using ImportShopApi.Extensions.Authentication;
using ImportShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace ImportShopApi.Controllers {
  [ApiController]
  [Route("auth")]
  public class AuthenticateController : Controller {
    private AccountService AccountService { get; }
    private IConfiguration Configuration { get; }

    public AuthenticateController(
      AccountService accountService,
      IConfiguration configuration
    ) {
      AccountService = accountService;
      Configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(AccountLogin accountLogin) {
      if (!ModelState.IsValid) return this.UnprocessableModelResult();

      var account = await AccountService.ByToken(accountLogin.TelegramToken);

      if (account == null)
        return this
          .AddModelError(MessageConstants.NoAccountWithCurrentToken)
          .UnprocessableModelResult();

      var botInfo = await new TelegramBotClient(account.TelegramToken).GetBotInfo();

      return Ok(new {
        Token = account.Id.CreateJwt(Configuration),
        Name = botInfo.Name,
        Avatar = botInfo.Avatar
      });
    }
  }
}