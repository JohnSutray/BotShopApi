using System.Collections.Generic;

namespace ImportShopApi.Models.Product
{
  public class Category
  {
    public string Name { get; set; }
    public IEnumerable<string> Types { get; set; }
  }
}