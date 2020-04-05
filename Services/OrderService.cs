using System.Linq;
using System.Threading.Tasks;
using ImportShopApi.Extensions.Dto;
using ImportShopApi.Models.Dto.Order;
using ImportShopCore;
using ImportShopCore.Attributes;
using ImportShopCore.Models;
using ImportShopCore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImportShopApi.Services {
  [Service]
  public class OrderService : RepositoryService<Order> {
    private ProductService ProductService { get; }
    private OrderItemService OrderItemService { get; }

    public OrderService(
      ApplicationContext context,
      ProductService productService,
      OrderItemService orderItemService
    ) : base(context, c => c.Orders) {
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

    private IQueryable<Order> FullyIncludeOrder(IQueryable<Order> query) =>
      query.Include(order => order.Chat)
        .Include(order => order.OrderItems)
        .ThenInclude(item => item.Product);
  }
}