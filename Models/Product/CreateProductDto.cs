using System.ComponentModel.DataAnnotations;
using ImportShopApi.Constants;
using Microsoft.AspNetCore.Http;

namespace ImportShopApi.Models.Product {
  public class CreateProductDto {
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [Range(1, 1000, ErrorMessage = ValidationMessages.Range)]
    public int Price { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "123")]
    [MaxLength(30, ErrorMessage = "Максимальная длина имени - 30 символов")]
    public string Name { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Описание продукта должно содержать как минимум один символ")]
    [MaxLength(5000, ErrorMessage = "Максимальная длина описания - 5000 символов")]
    public string Description { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Категория продукта должна содержать как минимум один символ")]
    [MaxLength(30, ErrorMessage = "Максимальная длина категории - 30 символов")]
    public string Category { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Категория продукта должна содержать как минимум один символ")]
    [MaxLength(30, ErrorMessage = "Максимальная длина категории - 30 символов")]
    public string Type { get; set; }

    public IFormFile Media { get; set; }
  }
}