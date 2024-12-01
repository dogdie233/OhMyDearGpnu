using AngleSharp.Html.Parser;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.AcaAff.Requests;

[Request(PayloadTypeEnum.FormUrlEncoded)]
public partial class LoginRequest : BaseRequest
{
    private readonly string password;

    [FormItem("yhm")] public string Username { get; init; }
    [FormItem("yzm")] public Captcha Captcha { get; init; }
    [FormItem("mm")] private string? EncryptedPassword { get; set; }
    [FormItem("language")] internal string Language => "zh_CN";

    /*[FormItem("csrftoken")]
    [FromPageCache("jwglxt/xtgl/login_slogin.html", selector: "#csrftoken")]
    private string? csrfToken;*/

    public override Uri Url => new(Hosts.acaAff, $"jwglxt/xtgl/login_slogin.html?time={Captcha.timestamp}");
    public override HttpMethod HttpMethod => HttpMethod.Post;

    public LoginRequest(string username, string password, Captcha captcha)
    {
        Username = username;
        this.password = password;
        Captcha = captcha;
    }

    public override async ValueTask FillAutoFieldAsync(SimpleServiceContainer serviceContainer)
    {
        await base.FillAutoFieldAsync(serviceContainer);
        var publicKeyResponse = await serviceContainer.Locate<GpnuClient>().SendRequest(new GetLoginPublicKeyRequest(Captcha.timestamp));

        var exponent = publicKeyResponse.Exponent;
        var modulus = publicKeyResponse.Modulus;
        EncryptedPassword = EncryptHelper.JwglxtPasswordEncrypt(password, exponent, modulus);
    }

    public override async ValueTask ValidResponse(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        if (responseMessage.RequestMessage!.RequestUri!.Host == "webauth.gpnu.edu.cn")
            throw new WebAuthRequiredException();

        var succeeded = responseMessage.RequestMessage!.RequestUri!.AbsolutePath == "/jwglxt/xtgl/index_initMenu.html";
        if (succeeded)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, Url);
            var cache = await PageCache.CreateFromResponseAsync(serviceContainer.Locate<GpnuClient>(), responseMessage, TimeSpan.FromMinutes(10), req);
            serviceContainer.Locate<PageCacheManager>().AddCache(cache);
        }

        var document = await new HtmlParser().ParseDocumentAsync(await responseMessage.Content.ReadAsStreamAsync());
        var tipsElement = document.QuerySelector("#tips");
        throw new UnexpectedResponseException(tipsElement != null ? tipsElement.TextContent.Trim() : "未知的错误");
    }
}