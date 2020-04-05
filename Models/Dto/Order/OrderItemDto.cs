using ImportShopApi.Models.Dto.Product;

namespace ImportShopApi.Models.Dto.Order {
  public class OrderItemDto {
    public ProductDto Product { get; set; }
    public int Amount { get; set; }
  }
}