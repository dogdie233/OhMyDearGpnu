using System.Net.Http.Json;
using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Cas.Responses;
using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.Cas.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class CheckScanRequest : BaseRequest<CheckScanResponse>
{
    [JsonIgnore] public override Uri Url => new(Hosts.cas, "lyuapServer/weChat/wx/CheckScan");

    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;

    [FormItem("state")] public string? State { get; set; }

    public override async ValueTask<CheckScanResponse> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        return (await responseMessage.Content.ReadFromJsonAsync(CasSourceGeneratedJsonContext.Default.CheckScanResponse))!;
    }
}