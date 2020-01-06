using System.Threading.Tasks;
using ImportShopBot.Extensions;
using ImportShopBot.Models;
using ImportShopBot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImportShopBot.Controllers
{
    [Authorize]
    [ApiController]
    [Route("account")]
    public class AccountController : Controller
    {
        private AccountService AccountService { get; }
        private ProductService ProductService { get; }

        public AccountController(
            AccountService accountService,
            ProductService productService
        )
        {
            AccountService = accountService;
            ProductService = productService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> CreateAccount(CreateBotAccountDto createBotAccountDto)
        {
            if (!ModelState.IsValid)
                return this.UnprocessableModel();

            if (await AccountService.IsCommonAccountExists(createBotAccountDto.TelegramToken))
                return this.AddModelError("Аккаунт с данным токеном уже существует").UnprocessableModel();
            
            
            await AccountService.CreateBotAccount(createBotAccountDto.TelegramToken);
            return Ok();
        }


        [HttpDelete]
        public async Task<ActionResult> RemoveAccount()
        {
            await ProductService.RemoveAllOwnerProducts(User.GetUserId());
            await AccountService.RemoveBotAccount(User.GetUserId());

            return Redirect("~/auth/sign-out");
        }
    }
}