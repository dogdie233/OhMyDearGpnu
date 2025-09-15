using System.Net;

using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.Cas.Requests;

[Request(PayloadTypeEnum.FormUrlEncoded)]
public partial class GetServiceTicketRequest(string tgt, string service) : BaseRequest<string>
{
    public override Uri Url => new(Hosts.cas, $"lyuapServer/v1/tickets/{tgt}");

    [FormItem("service")] private string Service { get; init; } = service;
    [FormItem("loginType")] private string LoginType => "loginToken";

    public override HttpMethod HttpMethod => HttpMethod.Post;

    public override async ValueTask<string> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        var content = await responseMessage.Content.ReadAsStringAsync();
        if (responseMessage.StatusCode == HttpStatusCode.InternalServerError && content.EndsWith("已失效"))
            throw new TokenExpiredException("TGT has expired.", typeof(CasHandler));
                        
        responseMessage.EnsureSuccessStatusCode();
        if (string.IsNullOrEmpty(content))
            throw new UnexpectedResponseException("Empty response content.");
        return content;
    }
}