namespace OhMyDearGpnu.Api.Modules
{
    public class Captcha : IDisposable
    {
        public readonly ulong timestamp;
        public readonly Stream imageStream;
        public string? value;

        internal Captcha(ulong timestamp, Stream imageStream)
        {
            this.timestamp = timestamp;
            this.imageStream = imageStream;
        }

        ~Captcha()
        {
            imageStream.Dispose();
        }

        public void Dispose()
        {
            imageStream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
