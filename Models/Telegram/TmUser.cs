using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ImportShopBot.Enums;

namespace ImportShopBot.Models.Telegram
{
    public class TmUser
    {
        public TmUser() => CartItems = new List<CartItem>();
        
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int TmId { get; set; }
        
        [Required]
        public int AccountId { get; set; }
        
        [Required]
        public EChatState ChatState { get; set; }
        
        public string LastSelectedCategory { get; set; }
        public string LastSelectedType { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}