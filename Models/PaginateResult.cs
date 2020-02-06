using System.Collections.Generic;

namespace ImportShopApi.Models
{
  public class PaginateResult<TItem>
  {
    public TItem[] Items { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public int TotalPages { get; set; }

    public bool IsFirstPage => Page == 0;
    public bool IsLastPage => Page == TotalPages - 1;
  }
}