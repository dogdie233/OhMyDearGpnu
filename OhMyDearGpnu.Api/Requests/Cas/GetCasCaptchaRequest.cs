using OhMyDearGpnu.Api.Modules.Cas;
using OhMyDearGpnu.Api.Responses;

using System.Net.Http.Json;
using System.Text.Json;

namespace OhMyDearGpnu.Api.Requests.Cas;

public class GetCasCaptchaRequest : BaseWithDataResponseRequest<CasCaptcha>
{
    private readonly string uid = Guid.NewGuid().ToString("N");

    public override string Path => $"lyuapServer/kaptcha?uid={uid}";
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override Task<DataResponse<CasCaptcha>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.Content.ReadFromJsonAsync<JsonDocument>();
        return new DataResponse<CasCaptcha>(null, new(Convert.FromBase64String()));
    }
}
