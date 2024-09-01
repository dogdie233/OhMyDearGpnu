using System.Net;

using OhMyDearGpnu.Api.Requests;
using OhMyDearGpnu.Api.Responses;

namespace OhMyDearGpnu.Api
{
    public class GpnuClient
    {
        public readonly SimpleServiceContainer serviceContainer;
        internal readonly HttpClient client;
        internal readonly CookieContainer cookieContainer;
        
        public bool IsLogin { get; internal set; }

        public GpnuClient()
        {
            cookieContainer = new CookieContainer();
            client = new HttpClient(new RedirectingHandler()
            {
                InnerHandler = new HttpClientHandler()
                {
                    AllowAutoRedirect = false,
                    CookieContainer = cookieContainer,
                    UseProxy = true
                },
                AllowAutoRedirect = true
            });
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36 Edg/118.0.0.0");

            serviceContainer = new SimpleServiceContainer();
            serviceContainer.AddExisted(this);
            serviceContainer.Register<PageCacheManager>();
        }

        public async Task<Response> SendRequest(BaseWithDataResponseRequest request)
        {
            await request.FillAutoFieldAsync(serviceContainer);
            var reqMsg = new HttpRequestMessage(request.HttpMethod, new Uri(request.Host + request.Path));
            var formItems = request.GetFormItems(serviceContainer);
            if (formItems.Any())
                reqMsg.Content = new FormUrlEncodedContent(formItems);
            var resMsg = await SendRequestMessageAsync(reqMsg);
            var res = await request.CreateResponseAsync(serviceContainer, resMsg);
            return res;
        }

        public async Task<DataResponse<TData>> SendRequest<TData>(BaseWithDataResponseRequest<TData> request)
        {
            await request.FillAutoFieldAsync(serviceContainer);
            var reqMsg = new HttpRequestMessage(request.HttpMethod, new Uri(request.Host + request.Path));
            var formItems = request.GetFormItems(serviceContainer);
            if (formItems.Any())
                reqMsg.Content = new FormUrlEncodedContent(formItems);
            var resMsg = await SendRequestMessageAsync(reqMsg);
            var res = await request.CreateDataResponseAsync(serviceContainer, resMsg);
            return res;
        }

        #region Low Level
        public void SetFormContent(HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> items)
        {
            request.Content = new FormUrlEncodedContent(items);
        }

        public async Task<HttpResponseMessage> SendRequestMessageAsync(HttpRequestMessage request)
        {
            var res = await client.SendAsync(request);
            return res;
        }
        #endregion
    }
}