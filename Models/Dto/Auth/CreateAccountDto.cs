using System.ComponentModel.DataAnnotations;

namespace BotShopApi.Models.Dto.Auth {
  public class CreateAccountDto {
    [Required]
    [MinLength(46)]
    [MaxLength(46)]
    public string TelegramToken { get; set; }
  }
}