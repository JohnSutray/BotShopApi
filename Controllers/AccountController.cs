using System.Threading.Tasks;
using BotShopApi.Constants;
using BotShopApi.Models.Dto.Auth;
using BotShopApi.Services;
using BotShopApi.Extensions;
using BotShopApi.Extensions.String;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BotShopApi.Controllers {
  [Authorize]
  [ApiController]
  [Route("account")]
  public class AccountController : Controller {
    private AccountService AccountService { get; }
    private ProductService ProductService { get; }

    public AccountController(
      AccountService accountService,
      ProductService productService
    ) {
      AccountService = accountService;
      ProductService = productService;
    }

    [HttpPut]
    [AllowAnonymous]
    public async Task<ActionResult> CreateAccount(CreateAccountDto createAccountDto) {
      if (!ModelState.IsValid)
        return this.UnprocessableModelResult();

      if (!await createAccountDto.TelegramToken.CheckTokenIsValidAsync())
        return this.AddModelError(Messages.InvalidToken)
          .UnprocessableModelResult();

      if (await AccountService.CheckIsAccountExistsAsync(createAccountDto.TelegramToken))
        return this.AddModelError(Messages.AccountWithCurrentTokenExists)
          .UnprocessableModelResult();

      await AccountService.CreateAccount(createAccountDto.TelegramToken);

      return Ok();
    }


    [HttpDelete]
    public async Task<ActionResult> DeleteAccount() {
      await ProductService.RemoveAllProductsAsync(User.GetUserId());
      await AccountService.RemoveAccount(User.GetUserId());

      return Ok();
    }
  }
}