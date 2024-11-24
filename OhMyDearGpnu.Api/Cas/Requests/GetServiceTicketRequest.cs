using System.Net;

using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.Cas.Requests;

[Request(PayloadTypeEnum.FormUrlEncoded)]
public partial class GetServiceTicketRequest(string tgt, string service) : BaseWithDataResponseRequest<string>
{
    public override Uri Url => new(Hosts.cas, $"lyuapServer/v1/tickets/{tgt}");

    [FormItem("service")] private string Service { get; init; } = service;
    [FormItem("loginType")] private string LoginType => "loginToken";

    public override HttpMethod HttpMethod => HttpMethod.Post;

    public override async Task<DataResponse<string>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        var content = (responseMessage.Content.Headers.ContentLength ?? 0) > 0 ? await responseMessage.Content.ReadAsStringAsync() : null;
        return responseMessage.StatusCode == HttpStatusCode.OK
            ? DataResponse<string>.Success(content ?? "")
            : DataResponse<string>.Fail(content ?? $"StatusCode is {responseMessage.StatusCode} instead of OK");
    }
}