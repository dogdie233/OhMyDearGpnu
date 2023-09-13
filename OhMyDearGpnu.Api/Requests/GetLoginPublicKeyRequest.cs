using OhMyDearGpnu.Api.Responses;

using System.Text.Json;

namespace OhMyDearGpnu.Api.Requests
{
    public class GetLoginPublicKeyRequest : BaseWithDataResponseRequest<GetLoginPublicKeyData>
    {
        public readonly string timestamp;

        public GetLoginPublicKeyRequest(string? timestamp = null)
        {
            if (timestamp == null)
                timestamp = (DateTime.Now - DateTime.UnixEpoch).TotalMilliseconds.ToString("F0");
            this.timestamp = timestamp;
        }

        public override string Path => $"jwglxt/xtgl/login_getPublicKey.html?time={timestamp}";
        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override async Task<DataResponse<GetLoginPublicKeyData>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
        {
            responseMessage.EnsureSuccessStatusCode();
            var content = await responseMessage.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<GetLoginPublicKeyData>(content);
            if (data != null)
                return new(null, data);
            return new("Json反序列化失败", null);
        }
    }
}
