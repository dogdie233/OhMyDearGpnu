using System.Net;

using OhMyDearGpnu.Api.Responses;

namespace OhMyDearGpnu.Api.Requests.Cas;

public class GetServiceTicketRequest(string tgt) : BaseWithDataResponseRequest<string>
{
    [FormItem("service")] private readonly string service = "https://webauth.gpnu.edu.cn/wengine-auth/login?cas_login=true";
    [FormItem("loginType")] private readonly string loginType = "loginToken";
    
    public override string Host => "https://cas.gpnu.edu.cn/";
    public override string Path => $"lyuapServer/v1/tickets/{tgt}";
    public override HttpMethod HttpMethod => HttpMethod.Post;

    public override async Task<DataResponse<string>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        if (responseMessage.StatusCode != HttpStatusCode.OK)
            return DataResponse<string>.Fail($"StatusCode is {responseMessage.StatusCode} instead of OK");
        var payload = await responseMessage.Content.ReadAsStringAsync();
        return DataResponse<string>.Success(payload);
    }
}