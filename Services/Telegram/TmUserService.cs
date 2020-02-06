using System.Linq;
using System.Threading.Tasks;
using ImportShopApi.Contexts;
using ImportShopApi.Extensions;
using ImportShopApi.Models.Telegram;
using Microsoft.EntityFrameworkCore;

namespace ImportShopApi.Services.Telegram {
  public class TmUserService {
    public TmUserService(TmUserContext userContext) => UserContext = userContext;
    public int AccountId { private get; set; }
    private TmUserContext UserContext { get; }

    private IQueryable<TmUser> Users => UserContext
      .TmUsers
      .Where(user => user.AccountId == AccountId);

    public async Task<TmUser> GetUserByTmId(int userId) =>
      await Users.FirstOrDefaultAsync(user => user.TmId == userId)
      ?? await UserContext.AddAndSaveAsync(new TmUser {TmId = userId, AccountId = AccountId});

    public async Task SaveChangesAsync() => await UserContext.SaveChangesAsync();
  }
}