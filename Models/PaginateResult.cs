using System.Collections.Generic;

namespace ImportShopBot.Models
{
    public class PaginateResult<TItem>
    {
        public IEnumerable<TItem> Items { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalPages { get; set; }
    }
}