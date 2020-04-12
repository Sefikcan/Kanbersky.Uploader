using System.IO;

namespace Kanbersky.Uploader.Business.DTO.Response
{
    public class UploadFileResponseModel
    {
        public Stream Content { get; set; }
        public string ContentType { get; set; }
    }
}
