using OhMyDearGpnu.Api.Requests;
using OhMyDearGpnu.Api.Responses;

namespace OhMyDearGpnu.Api.Cas.Requests;

[Request(PayloadTypeEnum.None)]
public partial class CheckCasRequest : BaseWithDataResponseRequest<bool>
{
    public override Uri Url { get; } = new(Hosts.acaAff, "jwglxt/xtgl/login_slogin.html");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override Task<DataResponse<bool>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        return Task.FromResult(DataResponse<bool>.Success("webauth.gpnu.edu.cn" == responseMessage.RequestMessage!.RequestUri!.Host));
    }
}