using ImportShopBot.Contexts;

namespace ImportShopBot.Telegram.Services
{
    public class TelegramProductService
    {
        private ProductContext ProductContext { get; }

        public TelegramProductService(ProductContext productContext)
            => ProductContext = productContext;
        
        
    }
}