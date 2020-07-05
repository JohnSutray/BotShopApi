using BotShopApi.Models.Dto.Product;

namespace BotShopApi.Models.Dto.Order {
  public class OrderItemDto {
    public ProductDto Product { get; set; }
    public int Amount { get; set; }
  }
}