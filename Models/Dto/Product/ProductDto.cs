namespace ImportShopApi.Models.Dto.Product {
  public class ProductDto {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public string MediaUrl { get; set; }
  }
}