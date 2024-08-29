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
