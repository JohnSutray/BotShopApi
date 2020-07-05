using System.Linq;
using System.Threading.Tasks;
using BotShopApi.Extensions.Dto;
using BotShopApi.Models.Dto.Order;
using BotShopCore;
using BotShopCore.Attributes;
using BotShopCore.Models;
using BotShopCore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BotShopApi.Services {
  [Service]
  public class OrderService : RepositoryService<Order> {
    private ProductService ProductService { get; }
    private OrderItemService OrderItemService { get; }

    public OrderService(
      ApplicationContext context,
      ProductService productService,
      OrderItemService orderItemService
    ) : base(context) {
      ProductService = productService;
      OrderItemService = orderItemService;
    }

    public async Task<PaginationResult<OrderDto>> Paginate(int accountId, int page, int limit) {
      var orders = await PaginateByPatternAsync(
        order => order.AccountId == accountId,
        FullyIncludeOrder,
        page,
        limit
      );


      return new PaginationResult<OrderDto> {
        Limit = orders.Limit,
        Page = orders.Page,
        Items = orders.Items.Select(OrderDtoExtensions.ToDto).ToList(),
        TotalPages = orders.TotalPages
      };
    }

    public async Task RemoveOrder(int orderId) => await RemoveByIdAsync(orderId);

    public async Task<bool> ValidateOrderId(int orderId) => await ByIdAsync(orderId) != null;

    private IQueryable<Order> FullyIncludeOrder(IQueryable<Order> query) =>
      query.Include(order => order.Chat)
        .Include(order => order.OrderItems)
        .ThenInclude(item => item.Product);

    protected override DbSet<Order> Set => Context.Orders;
  }
}
