using System.ComponentModel.DataAnnotations;

namespace ImportShopBot.Models
{
    public class CreateBotAccountDto
    {
        [Required]
        [MinLength(46)]
        [MaxLength(46)]
        public string TelegramToken { get; set; }
    }
}