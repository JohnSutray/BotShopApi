using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using ImportShopCore.Extensions.Media;
using ImportShopApi.Extensions.Aws;
using ImportShopCore.Attributes;
using Microsoft.AspNetCore.Http;

namespace ImportShopApi.Services {
  [Service]
  public class MediaStorageService {
    private IAmazonS3 AmazonS3 { get; }

    public MediaStorageService(IAmazonS3 amazonS3) {
      AmazonS3 = amazonS3;
    }

    private string CreateGuid() => Guid.NewGuid().ToString();

    private string BucketName => "import-shop-bot-dev";

    public async Task<string> UploadMedia(IFormFile file, int ownerId) {
      var putObjectRequest = new PutObjectRequest {
        Key = $"{ownerId}/{CreateGuid()}{Path.GetExtension(file.FileName)}",
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