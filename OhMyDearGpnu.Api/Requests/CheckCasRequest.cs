using OhMyDearGpnu.Api.Responses;

namespace OhMyDearGpnu.Api.Requests;

public class CheckCasRequest : BaseWithDataResponseRequest<bool>
{
    public override string Path => "jwglxt/xtgl/login_slogin.html";
    public override HttpMethod HttpMethod => HttpMethod.Get;
    public override Task<DataResponse<bool>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
        => Task.FromResult(DataResponse<bool>.Success("webauth.gpnu.edu.cn" == responseMessage.RequestMessage!.RequestUri!.Host));
}