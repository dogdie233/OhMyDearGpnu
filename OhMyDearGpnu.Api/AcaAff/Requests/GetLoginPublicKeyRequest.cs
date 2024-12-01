using System.Text.Json;

using OhMyDearGpnu.Api.AcaAff.Responses;
using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.AcaAff.Requests;

[Request(PayloadTypeEnum.None)]
public partial class GetLoginPublicKeyRequest : BaseRequest<GetLoginPublicKeyData>
{
    public readonly ulong timestamp;

    public GetLoginPublicKeyRequest(ulong timestamp)
    {
        this.timestamp = timestamp;
    }

    public override Uri Url => new(Hosts.acaAff, $"jwglxt/xtgl/login_getPublicKey.html?time={timestamp}");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override async ValueTask<GetLoginPublicKeyData> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        var content = await responseMessage.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<GetLoginPublicKeyData>(content);
        if (data == null)
            throw new UnexpectedResponseException("Failed to deserialize the response data.");
        return data;
    }
}