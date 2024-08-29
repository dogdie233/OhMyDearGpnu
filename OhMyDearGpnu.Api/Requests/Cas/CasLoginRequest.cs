using OhMyDearGpnu.Api.Modules.Cas;
using OhMyDearGpnu.Api.Responses;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Requests.Cas;

public class CasLoginRequest : BaseWithDataResponseRequest
{
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
    
    public CasLoginRequest(string username, string password, CasCaptcha captcha)
    {
        this.username = username;
        this.password = password;
        this.captcha = captcha;
        this.id = this.captcha.uid.ToString("N");
    }

    public override Task FillAutoFieldAsync(SimpleServiceContainer serviceContainer)
    {
        var pageCacheManager = serviceContainer.Locate<PageCacheManager>();
        pageCacheManager.GetCache("")
        
        encryptedPassword = Convert.ToHexString(EncryptHelper.EncryptPkcs1(password, ));
        return base.FillAutoFieldAsync(serviceContainer);
    }

    public override Task<Response> CreateResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        
    }
}