using System.Text.Json;

using OhMyDearGpnu.Api.AcaAff.Responses;
using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.AcaAff.Requests;

[Request(PayloadTypeEnum.None)]
public partial class GetLoginPublicKeyRequest : BaseWithDataResponseRequest<GetLoginPublicKeyData>
{
    public readonly ulong timestamp;

    public GetLoginPublicKeyRequest(ulong timestamp)
    {
        this.timestamp = timestamp;
    }

    public override Uri Url => new(Hosts.acaAff, $"jwglxt/xtgl/login_getPublicKey.html?time={timestamp}");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override async Task<DataResponse<GetLoginPublicKeyData>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        var content = await responseMessage.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<GetLoginPublicKeyData>(content);
        return data != null
            ? DataResponse<GetLoginPublicKeyData>.Success(data)
            : DataResponse<GetLoginPublicKeyData>.Fail("Json反序列化失败");
    }
}