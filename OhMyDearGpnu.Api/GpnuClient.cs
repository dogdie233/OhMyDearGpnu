namespace OhMyDearGpnu.Api
{
    public class GpnuClient
    {
        public readonly SimpleServiceContainer serviceContainer;
        internal HttpClient client;
        public bool IsLogin { get; private set; }

        public GpnuClient()
        {
            client = new(new HttpClientHandler()
            {
                AllowAutoRedirect = true,
                UseProxy = true
            })
            {
                BaseAddress = new Uri("https://jwglxt.gpnu.edu.cn/")
            };
            serviceContainer = new SimpleServiceContainer();
        }

        public async Task Login(string username, string password, Captcha captcha)
        {

        }

        public async Task<Captcha> CreateCaptchaAsync()
        {
            var timestamp = (DateTime.Now - DateTime.UnixEpoch).TotalMilliseconds.ToString();
            var req = new HttpRequestMessage(HttpMethod.Get, $"jwglxt/kaptcha?time={timestamp}");
            var res = await SendRequest(req);
            res.EnsureSuccessStatusCode();
            MemoryStream? ms = null;
            try
            {
                ms = new MemoryStream((int)(res.Content.Headers.ContentLength ?? 512));
                await res.Content.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
            }
            catch
            {
                ms?.Dispose();
                throw;
            }
            return new Captcha(timestamp, ms);
        }

        #region Low Level
        public void SetFormContent(HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> items)
        {
            request.Content = new FormUrlEncodedContent(items);
        }

        public async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
        {
            if (request.RequestUri?.Host != "jwglxt.gpnu.edu.cn")
                throw new UriFormatException("Host must gpnu");
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36 Edg/118.0.0.0");
            var res = await client.SendAsync(request);
            ListenResponse(res);
            return res;
        }

        private void ListenResponse(HttpResponseMessage response)
        {
            var request = response.RequestMessage!;
        }
        #endregion
    }
}
