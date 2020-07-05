using System.ComponentModel.DataAnnotations;

namespace BotShopApi.Models.Dto.Auth {
  public class AuthDto {
    [Required]
    [StringLength(46, ErrorMessage = "Длина Telegram токена составляет 46 символов")]
    public string TelegramToken { get; set; }
  }
}