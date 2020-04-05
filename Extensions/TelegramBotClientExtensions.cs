using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImportShopApi.Constants;
using ImportShopApi.Models;
using Telegram.Bot;

namespace ImportShopApi.Extensions {
  public static class TelegramBotClientExtensions {
    public static async Task<TmBotInfo> GetBotInfo(this TelegramBotClient client) {
      try {
        var primaryInfo = await client.GetMeAsync();

        return new TmBotInfo {
          Name = primaryInfo.FirstName,
          Avatar = await client.GetBotAvatar(primaryInfo.Id),
        };
      }
      catch {
        return null;
      }
    }

    private static async Task<string> GetBotAvatar(this TelegramBotClient client, int botId) {
      var allPhotos = await client.GetUserProfilePhotosAsync(botId);
      var firstPhotoId = allPhotos.Photos.FirstOrDefault()?.FirstOrDefault()?.FileId;


      if (firstPhotoId == null) {
        return null;
      }

      var photoTelegramFile = await client.GetFileAsync(firstPhotoId);
      await using var photoFileStream = new MemoryStream();
      await client.DownloadFileAsync(photoTelegramFile.FilePath, photoFileStream);

      return photoFileStream.ToBase64(EncodingConstants.ImageBase64Prefix);
    }

    public static string ToBase64(this MemoryStream stream, string dataTypePrefix) {
      return dataTypePrefix + Convert.ToBase64String(stream.ToArray());
    }
  }
}