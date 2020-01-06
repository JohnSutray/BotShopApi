using System.ComponentModel.DataAnnotations;

namespace ImportShopBot.Models
{
    public class BotAccount
    {
        public int Id { get; set; }

        [Required]
        [StringLength(46, ErrorMessage = "Длина Telegram токена составляет 46 символов")]
        public string TelegramToken { get; set; }

        [Required] public bool IsActive { get; set; } = true;
    }
}