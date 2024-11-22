using System.Net;

using OhMyDearGpnu.Api.Requests;
using OhMyDearGpnu.Api.Responses;

namespace OhMyDearGpnu.Api.Cas.Requests;

[Request(PayloadTypeEnum.FormUrlEncoded)]
public partial class GetServiceTicketRequest(string tgt) : BaseWithDataResponseRequest<string>
{
    public override Uri Url => new(Hosts.cas, $"lyuapServer/v1/tickets/{tgt}");

    [FormItem("service")] private string Service => "https://webauth.gpnu.edu.cn/wengine-auth/login?cas_login=true";
    [FormItem("loginType")] private string LoginType => "loginToken";

    public override HttpMethod HttpMethod => HttpMethod.Post;

    public override async Task<DataResponse<string>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        if (responseMessage.StatusCode != HttpStatusCode.OK)
            return DataResponse<string>.Fail($"StatusCode is {responseMessage.StatusCode} instead of OK");
        var payload = await responseMessage.Content.ReadAsStringAsync();
        return DataResponse<string>.Success(payload);
    }
}