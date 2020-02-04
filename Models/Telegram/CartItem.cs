using System.ComponentModel.DataAnnotations;

namespace ImportShopBot.Models.Telegram
{
    public class CartItem
    {
        public int Id { get; set; }
        [Required]
        public Product.Product Product { get; set; }
        [Required]
        public TmUser TmUser { get; set; }
    }
}