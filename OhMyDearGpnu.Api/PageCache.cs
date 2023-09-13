using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace OhMyDearGpnu.Api
{
    public class PageCache
    {
        private readonly GpnuClient client;
        private readonly HttpRequestMessage requestMessage;
        private readonly string identifier;
        private readonly TimeSpan expireTime;

        private string? htmlText;
        private IDocument? document;

        public string? HtmlText
        {
            get => IsExpire ? null : htmlText;
            private set => htmlText = value;
        }
        public string Identifier => identifier;
        public DateTime ExpireAt { get; private set; }
        public TimeSpan ExpireTime => expireTime;
        public IDocument? Document
        {
            get => IsExpire ? null : document;
            private set => document = value;
        }
        public bool IsExpire => DateTime.Now > ExpireAt;

        public PageCache(GpnuClient client, HttpRequestMessage requestMessage, TimeSpan expireTime)
        {
            this.client = client;
            this.requestMessage = requestMessage;
            this.identifier = requestMessage.RequestUri!.ToString();
            this.expireTime = expireTime;
            ExpireAt = DateTime.UnixEpoch;
        }

        public static PageCache CreateLazy(GpnuClient client, HttpRequestMessage req, TimeSpan expireTime)
            => new PageCache(client, req, expireTime);

        public static async Task<PageCache> CreateFromResponseAsync(GpnuClient client, HttpResponseMessage res, TimeSpan expireTime, HttpRequestMessage updateMethod)
        {
            var result = new PageCache(client, updateMethod, expireTime);
            await result.UpdateFromResponseAsync(res);
            return result;
        }

        private async Task UpdateFromResponseAsync(HttpResponseMessage res)
        {
            res.EnsureSuccessStatusCode();
            if (res.Content.Headers.ContentType!.MediaType != "text/html")
                throw new HttpRequestException("Content is not html");
            var content = await res.Content.ReadAsStringAsync();
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(content);
            htmlText = content;
            this.document = document;
            ExpireAt = DateTime.Now + expireTime;
        }

        public async Task UpdateAsync()
        {
            var res = await client.SendRequestMessageAsync(requestMessage);
            await UpdateFromResponseAsync(res);
        }

        public void MarkExpired()
        {
            ExpireAt = DateTime.Now.AddSeconds(-1);
        }
    }
}
