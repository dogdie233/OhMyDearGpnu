using System.Net;

using OhMyDearGpnu.Api.Cas;
using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api;

public sealed class GpnuClient
{
    public readonly SimpleServiceContainer serviceContainer;
    public readonly CasHandler cas;
    internal readonly HttpClient client;
    internal readonly CookieContainer cookieContainer;

    public GpnuClient()
    {
        cas = new CasHandler(this);
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
        RegisterServices();
    }

    private void RegisterServices()
    {
        serviceContainer.AddExisted(this);
        serviceContainer.AddExisted(cas);

        serviceContainer.Register<PageCacheManager>();
        serviceContainer.Register<IoT.IoTContext>(static container => IoT.IoTContext.CreateByServiceTicket(container.Locate<GpnuClient>()));
        serviceContainer.Register<AcaAff.AcaAffContext>(static container => AcaAff.AcaAffContext.CreateByServiceTicket(container.Locate<GpnuClient>()));
        serviceContainer.Register<ICasCaptchaResolver>(static _ => new SimpleCasCaptchaResolver());
    }

    public async Task SendRequest(BaseRequest request)
    {
        await request.FillAutoFieldAsync(serviceContainer);
        var reqMsg = new HttpRequestMessage(request.HttpMethod, request.Url);
        reqMsg.Headers.Authorization = request.GetAuthenticationHeaderValue(serviceContainer);
        reqMsg.Content = request.CreateHttpContent(serviceContainer);
        var resMsg = await SendRequestMessageAsync(reqMsg);
        await request.EnsureResponse(serviceContainer, resMsg);
    }

    public async Task<TData> SendRequest<TData>(BaseRequest<TData> request)
    {
        await request.FillAutoFieldAsync(serviceContainer);
        var reqMsg = new HttpRequestMessage(request.HttpMethod, request.Url);
        reqMsg.Headers.Authorization = request.GetAuthenticationHeaderValue(serviceContainer);
        reqMsg.Content = request.CreateHttpContent(serviceContainer);
        var resMsg = await SendRequestMessageAsync(reqMsg);
        await request.EnsureResponse(serviceContainer, resMsg);
        var res = await request.CreateDataResponseAsync(serviceContainer, resMsg);
        return res;
    }

    #region Low Level

    public async Task<HttpResponseMessage> SendRequestMessageAsync(HttpRequestMessage request)
    {
        var res = await client.SendAsync(request);
        return res;
    }

    #endregion
}