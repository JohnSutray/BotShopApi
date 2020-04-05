using System.Threading.Tasks;
using ImportShopApi.Constants;
using ImportShopApi.Extensions;
using ImportShopApi.Models.Dto.Auth;
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
    public async Task<ActionResult<AuthResultDto>> GetToken(AuthDto authDto) {
      if (!ModelState.IsValid) return this.UnprocessableModelResult();

      var account = await AccountService.ByToken(authDto.TelegramToken);

      if (account == null)
        return this
          .AddModelError(Messages.NoAccountWithCurrentToken)
          .UnprocessableModelResult();

      var botInfo = await new TelegramBotClient(account.TelegramToken).GetBotInfo();

      return new AuthResultDto {
        Token = account.Id.CreateJwt(Configuration),
        Name = botInfo.Name,
        Avatar = botInfo.Avatar
      };
    }
  }
}