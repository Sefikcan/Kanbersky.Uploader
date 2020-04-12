using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Kanbersky.Uploader.Business.Abstract;
using Kanbersky.Uploader.Business.DTO.Response;
using Kanbersky.Uploader.Core.Extensions;
using Kanbersky.Uploader.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Kanbersky.Uploader.Business.Concrete.Azure
{
    public class BlobStorageService : IFileUploaderService
    {
        #region fields

        private readonly BlobServiceClient _blobServiceClient;
        private readonly IOptions<AzureBlobSettings> _settings;

        #endregion

        #region ctor

        public BlobStorageService(BlobServiceClient blobServiceClient,
            IOptions<AzureBlobSettings> settings)
        {
            _settings = settings;
            _blobServiceClient = blobServiceClient;
        }

        #endregion

        #region methods

        public async Task DeleteFileAsync(string blobName)
        {
            var azureContainerClient = _settings.Value?.ContainerClient;
            var containerClient = _blobServiceClient.GetBlobContainerClient(azureContainerClient);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<UploadFileResponseModel> GetByFileNameUploadAsync(string fileName)
        {
            //Buraya ilk olarak db'den kayıt var mı kontrolü eklenebilir
            var azureContainerClient = _settings.Value?.ContainerClient;
            var containerClient = _blobServiceClient.GetBlobContainerClient(azureContainerClient);
            var blobClient = containerClient.GetBlobClient(fileName);
            var blobDownloadInfo = await blobClient.DownloadAsync();

            return new UploadFileResponseModel() 
            {
                Content = blobDownloadInfo.Value.Content,
                ContentType = blobDownloadInfo.Value.ContentType
            };
        }

        public async Task<IEnumerable<string>> GetAllUploadAsync()
        {
            var azureContainerClient = _settings.Value?.ContainerClient;
            var containerClient = _blobServiceClient.GetBlobContainerClient(azureContainerClient);
            var items = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                items.Add(blobItem.Name);
            }

            return items;
        }

        public async Task UploadContentAsync(string fileName, string content)
        {
            var azureContainerClient = _settings.Value?.ContainerClient;
            var containerClient = _blobServiceClient.GetBlobContainerClient(azureContainerClient);
            var blobClient = containerClient.GetBlobClient(fileName);
            var bytes = Encoding.UTF8.GetBytes(content);
            await using var memoryStream = new MemoryStream(bytes);
            await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders
            {
                ContentType = fileName.GetContentType()
            });
        }

        public async Task UploadFileAsync(string fileName, string filePath)
        {
            var azureContainerClient = _settings.Value?.ContainerClient;
            var containerClient = _blobServiceClient.GetBlobContainerClient(azureContainerClient);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(filePath, new BlobHttpHeaders 
            {
                ContentType = filePath.GetContentType()
            });
        }

        #endregion
    }
}
