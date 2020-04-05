using ImportShopApi.Models.Dto.Product;

namespace ImportShopApi.Extensions.Dto {
  public static class ProductDtoExtensions {
    public static ProductDto ToDto(this ImportShopCore.Models.Entities.Product product)
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