using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ImportShopApi.Models.Dto.Product {
  public class CreateProductDto {
    [Required]
    [Range(float.MinValue, float.MaxValue)]
    public float Price { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(byte.MaxValue)]
    public string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(5000)]
    public string Description { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(byte.MaxValue)]
    public string Category { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(byte.MaxValue)]
    public string Type { get; set; }

    [Required] public IFormFile Media { get; set; }
  }
}