using System.Collections.Generic;

namespace BotShopApi.Models.Dto.Product {
  public class CategoryDto {
    public string Name { get; set; }
    public List<string> Types { get; set; }
  }
}