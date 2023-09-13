using AngleSharp;
using AngleSharp.Dom;

namespace OhMyDearGpnu.Api
{
    public class PageCache
    {
        private readonly GpnuClient client;
        private readonly string uri;
        private readonly HttpRequestMessage requestMessage;
        private readonly TimeSpan expireTime;
        private IBrowsingContext browsingContext;

        public string HtmlText { get; private set; }
        public string Uri => uri;
        public DateTime ExpireAt { get; private set; }
        public TimeSpan ExpireTime => expireTime;
        public IBrowsingContext BrowsingContext => browsingContext;
        public IDocument Document { get; private set; }
        public bool IsExpire => DateTime.Now > ExpireAt;

        public PageCache(GpnuClient client, string htmlText, string uri, DateTime expireAt, TimeSpan expireTime, IBrowsingContext browsingContext, IDocument document, HttpRequestMessage requestMessage)
        {
            this.client = client;
            HtmlText = htmlText;
            this.uri = uri;
            ExpireAt = expireAt;
            this.expireTime = expireTime;
            this.browsingContext = browsingContext;
            Document = document;
            this.requestMessage = requestMessage;
        }

        public static async Task<PageCache> CreateFromBodyAsync(GpnuClient client, HttpResponseMessage res, TimeSpan expireTime)
        {
            res.EnsureSuccessStatusCode();
            var htmlText = await res.Content.ReadAsStringAsync();
            var config = Configuration.Default;
            var browsingContext = AngleSharp.BrowsingContext.New(config);
            var document = await browsingContext.OpenAsync(req => req.Address(res.RequestMessage!.RequestUri).Content(htmlText));
            var cache = new PageCache(client, htmlText, res.RequestMessage!.RequestUri!.ToString(), DateTime.Now + expireTime, expireTime, browsingContext, document, res.RequestMessage);
            return cache;
        }

        public async Task Update()
        {
            var res = await client.SendRequestMessage(requestMessage);
            res.EnsureSuccessStatusCode();
            Document.Close();
            browsingContext.Active = null;
            HtmlText = await res.Content.ReadAsStringAsync();
            Document = await browsingContext.OpenAsync(req => req.Address(requestMessage.RequestUri).Content(HtmlText));
            ExpireAt = DateTime.Now + expireTime;
        }

        public void MarkExpired()
        {
            ExpireAt = DateTime.Now.AddSeconds(-1);
        }
    }
}
