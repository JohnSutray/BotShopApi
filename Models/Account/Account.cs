using System.ComponentModel.DataAnnotations;

namespace ImportShopBot.Models.Account
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [StringLength(46, ErrorMessage = "Длина Telegram токена составляет 46 символов")]
        public string TelegramToken { get; set; }

        [Required] public bool IsActive { get; set; } = true;
    }
}