using System.ComponentModel.DataAnnotations;
using ImportShopBot.Constants;
using Microsoft.AspNetCore.Http;

namespace ImportShopBot.Models.Product
{
    public class UpdateProductDto
    {
        
        [Range(1, 1000, ErrorMessage = ValidationMessages.Price + ValidationMessages.Range)]
        public int? Price { get; set; }
        
        [MinLength(1, ErrorMessage = ValidationMessages.Name + ValidationMessages.MinLength)]
        [MaxLength(100, ErrorMessage = ValidationMessages.Name + ValidationMessages.MaxLength)]
        public string Name { get; set; }
        
        [MinLength(1, ErrorMessage = ValidationMessages.Description + ValidationMessages.MinLength)]
        [MaxLength(5000, ErrorMessage = ValidationMessages.Description + ValidationMessages.MaxLength)]
        public string Description { get; set; }
        
        [MinLength(1, ErrorMessage = ValidationMessages.Category + ValidationMessages.MinLength)]
        [MaxLength(100, ErrorMessage = ValidationMessages.Category + ValidationMessages.MaxLength)]
        public string Category { get; set; }
        
        [MinLength(1, ErrorMessage = ValidationMessages.Type + ValidationMessages.MinLength)]
        [MaxLength(100, ErrorMessage = ValidationMessages.Type + ValidationMessages.MaxLength)]
        public string Type { get; set; }

        // [FileExtensions(Extensions = "jpg,png,jpeg,mp4")]
        public IFormFile Media { get; set; }
    }
}