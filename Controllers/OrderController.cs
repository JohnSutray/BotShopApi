using System.Threading.Tasks;
using ImportShopApi.Extensions;
using ImportShopApi.Models.Dto.Order;
using ImportShopApi.Services;
using ImportShopCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImportShopApi.Controllers {
  [Authorize]
  [ApiController]
  [Route("order")]
  public class OrderController : Controller {
    private OrderService OrderService { get; }

    public OrderController(OrderService orderService) => OrderService = orderService;

    [HttpGet("{page}/{limit}")]
    public async Task<PaginationResult<OrderDto>> GetOrders(int page, int limit)
      => await OrderService.Paginate(User.GetUserId(), page, limit);

    [AllowAnonymous]
    [HttpGet]
    public string Test() {
      return "test";
    }
  }
}