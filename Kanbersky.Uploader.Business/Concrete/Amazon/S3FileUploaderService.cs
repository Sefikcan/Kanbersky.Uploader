using Amazon.S3;
using Amazon.S3.Model;
using Kanbersky.Uploader.Business.Abstract;
using Kanbersky.Uploader.Business.DTO.Response;
using Kanbersky.Uploader.Core.Extensions;
using Kanbersky.Uploader.Core.Results.Exceptions;
using Kanbersky.Uploader.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Kanbersky.Uploader.Business.Concrete.Amazon
{
    public class S3FileUploaderService : IFileUploaderService
    {
        #region fields

        private readonly AmazonS3Client _amazonS3Client;
        private readonly IOptions<AmazonBlobSettings> _settings;
        private readonly string _bucketName;

        #endregion

        #region ctor

        public S3FileUploaderService(IOptions<AmazonBlobSettings> settings)
        {
            _settings = settings;
            _bucketName = _settings.Value.Bucket;
            var accessSecret = _settings.Value.AccessSecret;
            var accessKey = _settings.Value.AccessKey;
            _amazonS3Client = new AmazonS3Client(accessKey, accessSecret);
        }

        #endregion

        #region methods

        public async Task DeleteFileAsync(string blobName)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = blobName
            };

            var response = await _amazonS3Client.DeleteObjectAsync(request);
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw BaseException.BadRequestException("Blob Remove Failed!");
            }
        }

        //Test et
        public async Task<UploadFileResponseModel> GetByFileNameUploadAsync(string fileName)
        {
            string contentType = string.Empty;
            Stream responseBody;

            //Buraya ilk olarak db'den kayıt var mı kontrolü eklenebilir
            var response = await _amazonS3Client.GetObjectAsync(_bucketName,fileName);
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                contentType = response.Headers["Content-Type"];
                responseBody = responseStream;
            }
            return new UploadFileResponseModel()
            {
                Content = responseBody,
                ContentType = contentType
            };
        }

        public async Task<IEnumerable<string>> GetAllUploadAsync()
        {
            var items = new List<string>();

            ListObjectsRequest request = new ListObjectsRequest
            {
                BucketName = _bucketName
            };
            ListObjectsResponse response = await _amazonS3Client.ListObjectsAsync(request);
            foreach (S3Object o in response.S3Objects)
            {
                items.Add(o.Key);
            }

            return items;
        }

        public Task UploadContentAsync(string fileName, string content)
        {
            throw BaseException.BadRequestException("Not Implemented!");
        }

        public async Task UploadFileAsync(string fileName, string filePath)
        {
            FileStream fileStream = File.OpenRead(filePath);
            byte[] fileBytes = new byte[fileStream.Length];

            fileStream.Read(fileBytes, 0, fileBytes.Length);
            fileStream.Close();

            //spesifik isim vermek istersen kullan
            //var fileName = Guid.NewGuid() + fileName

            PutObjectResponse response = null;

            using (var stream = new MemoryStream(fileBytes))
            {
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = stream,
                    ContentType = filePath.GetContentType(),
                    CannedACL = S3CannedACL.PublicRead
                };

                response = await _amazonS3Client.PutObjectAsync(request);
            };

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw BaseException.BadRequestException("S3 Upload Failed!");
            }
        }

        #endregion
    }
}
