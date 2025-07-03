using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;

using OhMyDearGpnu.Api.Cas.Responses;
using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Cas.Requests;

[Request(PayloadTypeEnum.FormUrlEncoded)]
public partial class LoginRequest : BaseRequest<LoginResponse>
{
    public static readonly byte[] publicExponent;
    public static readonly byte[] modulus;

    private readonly string password;

    [FormItem("username")] public string Username { get; init; }
    [FormItem("password")] private string? EncryptedPassword { get; set; }
    [FormItem("service")] public string Service { get; init; }
    [FormItem("loginType")] public string LoginType { get; set; }
    [FormItem("id")] private string Id { get; init; }
    [FormItem("code")] public CasCaptcha? Captcha { get; init; }

    public override Uri Url => new(Hosts.cas, "lyuapServer/v1/tickets");
    public override HttpMethod HttpMethod => HttpMethod.Post;

    static LoginRequest()
    {
        // TODO: Read from https://cas.gpnu.edu.cn/assets/js/app.523338dac63532ba3269.js
        // The js load from https://cas.gpnu.edu.cn/lyuapServer/login
        publicExponent = Convert.FromHexString("010001");
        modulus = Convert.FromHexString("00b5eeb166e069920e80bebd1fea4829d3d1f3216f2aabe79b6c47a3c18dcee5fd22c2e7ac519cab59198ece036dcf289ea8201e2a0b9ded307f8fb704136eaeb670286f5ad44e691005ba9ea5af04ada5367cd724b5a26fdb5120cc95b6431604bd219c6b7d83a6f8f24b43918ea988a76f93c333aa5a20991493d4eb1117e7b1");
    }

    public LoginRequest(string username, string password, CasCaptcha captcha, string service)
    {
        Username = username;
        LoginType = string.Empty;
        this.password = password;
        Captcha = captcha;
        Id = Captcha.uid.ToString("N");
        Service = service;
        EncryptedPassword = EncryptHelper.CasPasswordEncrypt(password, publicExponent, modulus);
    }

    public LoginRequest(string username, string password, string service)
    {
        Username = username;
        this.password = password;
        Id = string.Empty;
        LoginType = "3";
        Service = service;
        this.EncryptedPassword = password;
    }

    public override async ValueTask FillAutoFieldAsync(SimpleServiceContainer serviceContainer)
    {
        await base.FillAutoFieldAsync(serviceContainer);
    }

    public override async ValueTask<LoginResponse> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        var payload = await responseMessage.Content.ReadFromJsonAsync(CasSourceGeneratedJsonContext.Default.LoginResponse);
        if (payload == null)
            throw new UnexpectedResponseException("Failed to deserialize response");
        return payload;
    }
}