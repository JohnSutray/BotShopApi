using System.Threading.Tasks;
using BotShopApi.Constants;
using BotShopApi.Models.Dto.Order;
using BotShopApi.Services;
using BotShopApi.Extensions;
using BotShopCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BotShopApi.Controllers {
  [Authorize]
  [ApiController]
  [Route("order")]
  public class OrderController : Controller {
    private OrderService OrderService { get; }

    public OrderController(OrderService orderService) => OrderService = orderService;

    [HttpGet("{page}/{limit}")]
    public async Task<PaginationResult<OrderDto>> GetOrders(int page, int limit)
      => await OrderService.Paginate(User.GetUserId(), page, limit);

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(int id) {
      if (!await OrderService.ValidateOrderId(id))
        return this.AddModelError(Messages.OrderWithSelectedIdNotExists)
          .UnprocessableModelResult();

      await OrderService.RemoveOrder(id);

      return Ok();
    }

    [AllowAnonymous]
    [HttpGet]
    public string Test() {
      return "test";
    }
  }
}