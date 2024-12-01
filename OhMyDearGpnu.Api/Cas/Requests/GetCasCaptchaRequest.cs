using System.Net.Http.Json;

using OhMyDearGpnu.Api.Cas.Responses;
using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Cas.Requests;

[Request(PayloadTypeEnum.None)]
public partial class GetCasCaptchaRequest : BaseRequest<CasCaptcha>
{
    public override Uri Url => new(Hosts.cas, "lyuapServer/kaptcha?uid=");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override async ValueTask<CasCaptcha> CreateDataResponseAsync(SimpleServiceContainer serviceContainer,
        HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        var res = await responseMessage.Content.ReadFromJsonAsync<GetCasCaptchaResponse>();
        if (res is not { Content.Length: > 22 })
            throw await UnexpectedResponseException.FromHttpContentAsync(responseMessage, "Result is not a base64 encoded string.");

        var imageData = Utils.DecodeBase64(res.Content.AsSpan()[22..]);
        var expireAt = DateTime.Now.AddSeconds(res.Timeout);
        return new CasCaptcha(res.Uid, imageData, expireAt);
    }
}