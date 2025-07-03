using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OhMyDearGpnu.Api.Cas.Responses;
using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.Cas.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class CreateQRcodeRequest : BaseRequest<CreateQRcodeResponse>
{
    public override Uri Url => new(Hosts.cas, "lyuapServer/weChat/CreateQRcode");

    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override async ValueTask<CreateQRcodeResponse> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        return (await responseMessage.Content.ReadFromJsonAsync(CasSourceGeneratedJsonContext.Default.CreateQRcodeResponse))!;
    }
}
