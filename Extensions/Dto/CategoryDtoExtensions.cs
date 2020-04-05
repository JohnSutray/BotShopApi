using ImportShopApi.Models.Dto.Product;
using ImportShopCore.Models;

namespace ImportShopApi.Extensions.Dto {
  public static class CategoryDtoExtensions {
    public static CategoryDto ToDto(this Category category) => new CategoryDto {
      Name = category.Name,
      Types = category.Types
    };
  }
}