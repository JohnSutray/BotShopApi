using System.Threading.Tasks;
using BotShopApi.Constants;
using BotShopApi.Models.Dto.Auth;
using BotShopApi.Services;
using BotShopApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;

namespace BotShopApi.Controllers {
  [ApiController]
  [Route("auth")]
  public class AuthenticateController : Controller {
    private AccountService AccountService { get; }
    private JwtService JwtService { get; }

    public AuthenticateController(
      AccountService accountService,
      JwtService jwtService
    ) {
      AccountService = accountService;
      JwtService = jwtService;
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
        Token = JwtService.CreateJwt(account.Id),
        Name = botInfo.Name,
        Avatar = botInfo.Avatar
      };
    }
  }
}
