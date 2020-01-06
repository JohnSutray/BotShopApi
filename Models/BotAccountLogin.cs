using System.ComponentModel.DataAnnotations;

namespace ImportShopBot.Models
{
    public class BotAccountLogin
    {
        [Required]
        [StringLength(46, ErrorMessage = "Длина Telegram токена составляет 46 символов")]
        public string TelegramToken { get; set; }
    }
}