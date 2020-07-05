using System.Linq;
using BotShopApi.Models.Dto.Order;
using BotShopCore.Models.Entities;

namespace BotShopApi.Extensions.Dto {
  public static class OrderDtoExtensions {
    public static OrderDto ToDto(this Order order) => new OrderDto {
      Id = order.Id.ToString(),
      Address = order.Chat.Address,
      Phone = order.Chat.Phone,
      CreatedAt = order.CreatedAt,
      FirstName = order.Chat.FirstName,
      LastName = order.Chat.LastName,
      Items = order.OrderItems.GroupBy(item => item.ProductId).Select(GroupToDto).ToList()
    };

    private static OrderItemDto GroupToDto(IGrouping<int, OrderItem> group) => new OrderItemDto {
      Amount = group.Count(),
      Product = group.First().Product.ToDto()
    };
  }
}