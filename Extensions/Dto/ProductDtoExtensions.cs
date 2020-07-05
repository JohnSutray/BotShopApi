using BotShopApi.Models.Dto.Product;

namespace BotShopApi.Extensions.Dto {
  public static class ProductDtoExtensions {
    public static ProductDto ToDto(this BotShopCore.Models.Entities.Product product)
      => new ProductDto {
        Id = product.Id.ToString(),
        Category = product.Category,
        Description = product.Description,
        Name = product.Name,
        Price = product.Price,
        Type = product.Type,
        MediaUrl = product.MediaUrl
      };
  }
}