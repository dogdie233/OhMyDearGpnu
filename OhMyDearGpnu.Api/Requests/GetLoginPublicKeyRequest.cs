using OhMyDearGpnu.Api.Responses;

using System.Text.Json;

namespace OhMyDearGpnu.Api.Requests;

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