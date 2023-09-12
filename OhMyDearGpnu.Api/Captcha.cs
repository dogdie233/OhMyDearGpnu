namespace OhMyDearGpnu.Api
{
    public class Captcha
    {
        public readonly string timestamp;
        public readonly Stream imageStream;
        public string? value;

        internal Captcha(string timestamp, Stream imageStream)
        {
            this.timestamp = timestamp;
            this.imageStream = imageStream;
        }
    }
}
