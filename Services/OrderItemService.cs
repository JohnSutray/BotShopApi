using System.Collections.Generic;
using System.Threading.Tasks;
using ImportShopCore;
using ImportShopCore.Attributes;
using ImportShopCore.Models;
using ImportShopCore.Models.Entities;

namespace ImportShopApi.Services {
  [Service]
  public class OrderItemService : RepositoryService<OrderItem> {
    public OrderItemService(ApplicationContext context) : base(context, c => c.OrderItems) { }

    public async Task<List<OrderItem>> GetOrderItems(int orderId) =>
      await ByPatternManyAsync(item => item.OrderId == orderId, item => item.Product);
  }
}