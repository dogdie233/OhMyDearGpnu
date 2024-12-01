using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.Cas.Requests;

[Request(PayloadTypeEnum.None)]
public partial class CheckCasRequest : BaseRequest<bool>
{
    public override Uri Url { get; } = new(Hosts.acaAff, "jwglxt/xtgl/login_slogin.html");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override ValueTask<bool> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        return ValueTask.FromResult("webauth.gpnu.edu.cn" == responseMessage.RequestMessage!.RequestUri!.Host);
    }
}