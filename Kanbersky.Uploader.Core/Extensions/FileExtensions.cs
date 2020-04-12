using Microsoft.AspNetCore.StaticFiles;

namespace Kanbersky.Uploader.Core.Extensions
{
    public static class FileExtensions
    {
        private static readonly FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();

        public static string GetContentType(this string fileName)
        {
            if (!provider.TryGetContentType(fileName,out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
