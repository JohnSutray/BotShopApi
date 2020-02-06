using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImportShopApi.Models.Telegram
{
    public delegate Task<bool> TmHandler<in TContext>(TContext context) 
        where TContext : TmContext;

    public class TmHandlerContainer<TContext> where TContext : TmContext
    {
        public Regex HandleBy { get; }
        public TmHandler<TContext> Handler { get; }

        public TmHandlerContainer(Regex handleBy, TmHandler<TContext> handler)
        {
            HandleBy = handleBy;
            Handler = handler;
        }
    }
}