using System.Threading.Tasks;
using ImportShopBot.Extensions;
using ImportShopBot.Models;
using ImportShopBot.Models.Account;
using ImportShopBot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImportShopBot.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("account")]
    public class AccountController : Controller
    {
        private AccountService AccountService { get; }
        private ProductService ProductService { get; }
        private TmApiService TmApiService { get; }
        private TmBotManagerService BotManagerService { get; }

        public AccountController(
            AccountService accountService,
            ProductService productService,
            TmBotManagerService botManagerService,
            TmApiService tmApiService
        )
        {
            AccountService = accountService;
            ProductService = productService;
            BotManagerService = botManagerService;
            TmApiService = tmApiService;
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult> CreateAccount(CreateAccountDto createAccountDto)
        {
            if (!ModelState.IsValid)
                return this.UnprocessableModel();

            if (!await TmApiService.IsValidToken(createAccountDto.TelegramToken))
                return this.AddModelError("Неверный токен").UnprocessableEntity();

            if (await AccountService.IsCommonAccountExists(createAccountDto.TelegramToken))
                return this.AddModelError("Аккаунт с данным токеном уже существует").UnprocessableModel();

            await AccountService.CreateBotAccount(createAccountDto.TelegramToken);
            BotManagerService.UpdateBots();

            return Ok();
        }


        [HttpDelete]
        public async Task<ActionResult> RemoveAccount()
        {
            await ProductService.RemoveAllOwnerProducts(User.GetUserId());
            await AccountService.RemoveAccount(User.GetUserId());

            return Ok();
        }
    }
}