using System.Net.Http.Json;

using OhMyDearGpnu.Api.Cas.Responses;
using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Cas.Requests;

[Request(PayloadTypeEnum.None)]
public partial class GetCasCaptchaRequest : BaseWithDataResponseRequest<CasCaptcha>
{
    public override Uri Url => new(Hosts.cas, "lyuapServer/kaptcha?uid=");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override async Task<DataResponse<CasCaptcha>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer,
        HttpResponseMessage responseMessage)
    {
        var res = await responseMessage.Content.ReadFromJsonAsync<GetCasCaptchaResponse>();
        if (res is not { Content.Length: > 22 })
            return DataResponse<CasCaptcha>.Fail(await responseMessage.Content.ReadAsStringAsync());
        var imageData = Utils.DecodeBase64(res.Content.AsSpan()[22..]);
        var expireAt = DateTime.Now.AddSeconds(res.Timeout);
        return DataResponse<CasCaptcha>.Success(new CasCaptcha(res.Uid, imageData, expireAt));
    }
}