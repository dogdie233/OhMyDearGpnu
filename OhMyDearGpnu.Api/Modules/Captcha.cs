namespace OhMyDearGpnu.Api.Modules
{
    public class Captcha : IDisposable
    {
        public readonly Stream imageStream;
        public string? value;

        internal Captcha(Stream imageStream)
        {
            this.imageStream = imageStream;
        }

        public void Dispose() => imageStream.Dispose();
    }
}
