using BotShopApi.Models.Dto.Product;
using BotShopCore.Models;

namespace BotShopApi.Extensions.Dto {
  public static class CategoryDtoExtensions {
    public static CategoryDto ToDto(this Category category) => new CategoryDto {
      Name = category.Name,
      Types = category.Types
    };
  }
}