using System.Collections.Generic;
using System.Threading.Tasks;
using BotShopCore;
using BotShopCore.Attributes;
using BotShopCore.Models;
using BotShopCore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BotShopApi.Services {
  [Service]
  public class OrderItemService : RepositoryService<OrderItem> {
    public OrderItemService(ApplicationContext context) : base(context) { }

    public async Task<List<OrderItem>> GetOrderItems(int orderId) =>
      await ByPatternManyAsync(item => item.OrderId == orderId, item => item.Product);

    protected override DbSet<OrderItem> Set => Context.OrderItems;
  }
}
