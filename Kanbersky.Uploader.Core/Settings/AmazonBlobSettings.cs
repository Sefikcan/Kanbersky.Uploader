namespace Kanbersky.Uploader.Core.Settings
{
    public class AmazonBlobSettings : ISettings
    {
        public string AccessKey { get; set; }

        public string AccessSecret { get; set; }

        public string Bucket { get; set; }
    }
}
