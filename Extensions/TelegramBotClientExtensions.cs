using System.Threading.Tasks;
using ImportShopBot.Enums;
using ImportShopBot.Extensions.String;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImportShopBot.Extensions
{
    public static class TelegramBotClientExtensions
    {
        public static async Task SendTextWithMarkupAsync(
            this TelegramBotClient client,
            int chatId,
            string text,
            IReplyMarkup markup
        ) => await client.SendTextMessageAsync(
            chatId,
            text,
            replyMarkup: markup
        );

        private static async Task SendPhotoWithMarkupAsync(
            this TelegramBotClient client,
            int chatId,
            InputOnlineFile file,
            string caption,
            IReplyMarkup markup
        ) => await client.SendPhotoAsync(
            chatId,
            file,
            caption,
            replyMarkup: markup
        );

        private static async Task SendVideoWithMarkupAsync(
            this TelegramBotClient client,
            int chatId,
            InputOnlineFile video,
            string caption,
            IReplyMarkup markup
        ) => await client.SendVideoAsync(
            chatId,
            video,
            caption: caption,
            replyMarkup: markup
        );

        public static async Task SendMediaWithMarkupAsync(
            this TelegramBotClient client,
            int chatId,
            InputOnlineFile imageOrVideo,
            string caption,
            IReplyMarkup markup
        )
        {
            switch (imageOrVideo.Url.GetDisplayType())
            {
                case EDisplayType.Video:
                    await client.SendVideoWithMarkupAsync(chatId, imageOrVideo, caption, markup);
                    break;
                case EDisplayType.Image:
                    await client.SendPhotoWithMarkupAsync(chatId, imageOrVideo, caption, markup);
                    break;
            }
        }
    }
}