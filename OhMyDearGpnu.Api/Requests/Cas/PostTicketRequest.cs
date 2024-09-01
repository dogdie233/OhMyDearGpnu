using System.Net;
using System.Net.Http.Json;

using OhMyDearGpnu.Api.Modules.Cas;
using OhMyDearGpnu.Api.Responses;
using OhMyDearGpnu.Api.Responses.Cas;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Requests.Cas;

public class PostTicketRequest : BaseWithDataResponseRequest<PostTicketResponse>
{
    public static readonly byte[] publicExponent;
    public static readonly byte[] modulus;
    
    [FormItem("username")] public readonly string username;
    public readonly string password;
    [FormItem("password")] private string? encryptedPassword;
    [FormItem("service")] public string service = "https://webauth.gpnu.edu.cn/wengine-auth/login?cas_login=true";
    [FormItem("loginType")] public string loginType = string.Empty;
    [FormItem("id")] private readonly string id;
    [FormItem("code")] public readonly CasCaptcha captcha;
    
    public override string Host => "https://cas.gpnu.edu.cn/";
    public override string Path => "lyuapServer/v1/tickets";
    public override HttpMethod HttpMethod => HttpMethod.Post;

    static PostTicketRequest()
    {
        publicExponent = Convert.FromHexString("010001");
        modulus = Convert.FromHexString("00b5eeb166e069920e80bebd1fea4829d3d1f3216f2aabe79b6c47a3c18dcee5fd22c2e7ac519cab59198ece036dcf289ea8201e2a0b9ded307f8fb704136eaeb670286f5ad44e691005ba9ea5af04ada5367cd724b5a26fdb5120cc95b6431604bd219c6b7d83a6f8f24b43918ea988a76f93c333aa5a20991493d4eb1117e7b1");
    }
    
    public PostTicketRequest(string username, string password, CasCaptcha captcha)
    {
        this.username = username;
        this.password = password;
        this.captcha = captcha;
        this.id = this.captcha.uid.ToString("N");
    }

    public override async Task FillAutoFieldAsync(SimpleServiceContainer serviceContainer)
    {
        await base.FillAutoFieldAsync(serviceContainer);
        encryptedPassword = EncryptHelper.CasPasswordEncrypt(password, publicExponent, modulus);
    }

    public override async Task<DataResponse<PostTicketResponse>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        if (responseMessage.StatusCode != HttpStatusCode.OK)
            return DataResponse<PostTicketResponse>.Fail($"StatusCode is {responseMessage.StatusCode} instead of OK");
        var payload = await responseMessage.Content.ReadFromJsonAsync<PostTicketResponse>();
        return payload == null
            ? DataResponse<PostTicketResponse>.Fail("Failed to deserialize response")
            : DataResponse<PostTicketResponse>.Success(payload);
    }
}