using OhMyDearGpnu.Api.Requests;
using OhMyDearGpnu.Api.Responses;

namespace OhMyDearGpnu.Api
{
    public class GpnuClient
    {
        public readonly SimpleServiceContainer serviceContainer;
        internal HttpClient client;

        public bool IsLogin { get; internal set; }

        public GpnuClient()
        {
            client = new(new HttpClientHandler()
            {
                UseProxy = true
            })
            {
                BaseAddress = new Uri("https://jwglxt.gpnu.edu.cn/")
            };

            serviceContainer = new SimpleServiceContainer();
            serviceContainer.AddExisted(this);
            serviceContainer.Register<PageCacheManager>();
        }

        public async Task<Response> SendRequest(BaseRequest request)
        {
            await request.FillAutoFieldAsync(serviceContainer);
            var reqMsg = new HttpRequestMessage(request.HttpMethod, request.Path);
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
            var reqMsg = new HttpRequestMessage(request.HttpMethod, request.Path);
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
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36 Edg/118.0.0.0");
            var res = await client.SendAsync(request);
            return res;
        }
        #endregion
    }
}