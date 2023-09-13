namespace OhMyDearGpnu.Api
{
    public class Captcha : IDisposable
    {
        public readonly string timestamp;
        public readonly Stream imageStream;
        public string? value;

        internal Captcha(string timestamp, Stream imageStream)
        {
            this.timestamp = timestamp;
            this.imageStream = imageStream;
        }

        public void Dispose() => imageStream.Dispose();
    }
}
