namespace Kanbersky.Uploader.Core.Settings
{
    public class AzureBlobSettings : ISettings
    {
        public string ConnectionStrings { get; set; }

        public string ContainerClient { get; set; }
    }
}
