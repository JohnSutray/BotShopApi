using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using BotShopApi.Constants;
using BotShopApi.Extensions.String;
using BotShopCore.Attributes;
using BotShopCore.Extensions.Media;
using Microsoft.AspNetCore.Http;

namespace BotShopApi.Services {
  [Service]
  public class MediaStorageService {
    private IAmazonS3 AmazonS3 { get; }

    public MediaStorageService(IAmazonS3 amazonS3) => AmazonS3 = amazonS3;

    private string BucketName => EnvironmentConstants.AwsBucketName;

    public async Task<string> UploadMedia(IFormFile file, int ownerId) {
      var putObjectRequest = new PutObjectRequest {
        Key = $"{ownerId}/{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}",
        BucketName = BucketName,
        InputStream = file.OpenReadStream(),
        ContentType = file.FileName.GetContentType(),
        CannedACL = S3CannedACL.PublicRead
      };

      await AmazonS3.PutObjectAsync(putObjectRequest);

      var request = new GetPreSignedUrlRequest {
        Key = putObjectRequest.Key,
        BucketName = putObjectRequest.BucketName,
        Protocol = Protocol.HTTPS,
        Expires = DateTime.Now
      };

      return AmazonS3.GetPreSignedURL(request).UrlWithoutQueryParams();
    }

    public async Task RemoveMedia(string mediaUrl) {
      var deleteObjectRequest = new DeleteObjectRequest {
        BucketName = BucketName,
        Key = mediaUrl.GetS3Key()
      };

      await AmazonS3.DeleteObjectAsync(deleteObjectRequest);
    }

    public async Task RemoveManyMedia(IEnumerable<string> keys) {
      var deleteObjectsRequest = new DeleteObjectsRequest {
        Objects = keys.Select(k => new KeyVersion {Key = k}).ToList(),
        BucketName = BucketName
      };

      await AmazonS3.DeleteObjectsAsync(deleteObjectsRequest);
    }
  }
}
