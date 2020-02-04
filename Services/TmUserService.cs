using System.Linq;
using System.Threading.Tasks;
using ImportShopBot.Contexts;
using ImportShopBot.Extensions;
using ImportShopBot.Models.Telegram;

namespace ImportShopBot.Services
{
    public class TmUserService
    {
        private TmUserContext UserContext { get; }

        private IQueryable<TmUser> Users => UserContext.TmUsers;

        public TmUserService(TmUserContext userContext) => UserContext = userContext;

        public async Task<TmUser> GetUserByTmId(int tmId, int accountId) =>
            Users.FirstOrDefault(user => user.TmId == tmId && user.AccountId == accountId) 
            ?? await UserContext.AddAndSaveAsync(new TmUser { TmId = tmId, AccountId = accountId});

        public async Task SaveChanges() => await UserContext.SaveChangesAsync();
    }
}