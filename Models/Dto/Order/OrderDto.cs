using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BotShopApi.Models.Dto.Order {
  public class OrderDto {
    [Required] public string Id { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
    [Required] public string FirstName { get; set; }
    public string LastName { get; set; }
    [Required] public string Phone { get; set; }
    public string Address { get; set; }

    public List<OrderItemDto> Items { get; set; }
  }
}