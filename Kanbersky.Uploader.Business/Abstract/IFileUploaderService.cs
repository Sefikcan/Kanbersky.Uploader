using Kanbersky.Uploader.Business.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kanbersky.Uploader.Business.Abstract
{
    public interface IFileUploaderService
    {
        Task<UploadFileResponseModel> GetByFileNameUploadAsync(string fileName);

        Task<IEnumerable<string>> GetAllUploadAsync();

        Task UploadFileAsync(string fileName, string filePath);

        Task UploadContentAsync(string fileName, string content);

        Task DeleteFileAsync(string blobName);
    }
}
