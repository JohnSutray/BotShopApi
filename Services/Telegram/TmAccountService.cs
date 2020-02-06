using System.Linq;
using ImportShopApi.Contexts;
using ImportShopApi.Models.Account;

namespace ImportShopApi.Services.Telegram {
  public class TmAccountService {
    public IQueryable<Account> Accounts => AccountContext.Accounts;
    private AccountContext AccountContext { get; }

    public TmAccountService(AccountContext accountContext)
      => AccountContext = accountContext;
  }
}