using System.ComponentModel.DataAnnotations;
using ImportShopApi.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ImportShopApi.Models.Product
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Price { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(5000)]
        public string Description { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Category { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Type { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public string MediaUrl { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int OwnerId { get; set; }
    }
}