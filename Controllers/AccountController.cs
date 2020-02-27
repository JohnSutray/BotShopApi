using System.Threading.Tasks;
using ImportShopCore.Models.Account;
using ImportShopApi.Extensions;
using ImportShopApi.Extensions.Authentication;
using ImportShopApi.Extensions.TelegramApi;
using ImportShopApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImportShopApi.Controllers {
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
        return this.AddModelError("Неверный токен").UnprocessableModelResult();

      if (await AccountService.CheckIsAccountExistsAsync(createAccountDto.TelegramToken))
        return this.AddModelError("Аккаунт с данным токеном уже существует").UnprocessableModelResult();

      await AccountService.CreateAccount(createAccountDto.TelegramToken);

      return Ok();
    }


    [HttpDelete]
    public async Task<ActionResult> RemoveAccount() {
      await ProductService.RemoveAllProductsAsync(User.GetUserId());
      await AccountService.RemoveAccount(User.GetUserId());

      return Ok();
    }
  }
}