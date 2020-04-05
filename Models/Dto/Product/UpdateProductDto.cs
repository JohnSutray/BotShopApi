using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ImportShopApi.Models.Dto.Product {
  public class UpdateProductDto {
    [Range(float.MinValue, float.MaxValue)]
    public float? Price { get; set; }

    [MinLength(1)]
    [MaxLength(byte.MaxValue)]
    public string Name { get; set; }

    [MinLength(1)]
    [MaxLength(byte.MaxValue)]
    public string Description { get; set; }

    [MinLength(1)] [MaxLength(5000)] public string Category { get; set; }

    [MinLength(1)]
    [MaxLength(byte.MaxValue)]
    public string Type { get; set; }

    public IFormFile Media { get; set; }
  }
}