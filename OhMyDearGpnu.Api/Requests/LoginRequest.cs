using System.Text;

using AngleSharp.Html.Parser;

using OhMyDearGpnu.Api.Modules;
using OhMyDearGpnu.Api.Responses;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Requests
{
    public class LoginRequest : BaseWithDataResponseRequest
    {
        [FormItem("yhm")]
        public string username;
        public string password;

        [FormItem("yzm")]
        public Captcha captcha;

        [FormItem("mm")]
        internal string? encryptedPassword;

        [FormItem("language")]
        internal string language = "zh_CN";

        /*[FormItem("csrftoken")]
        [FromPageCache("jwglxt/xtgl/login_slogin.html", selector: "#csrftoken")]
        private string? csrfToken;*/
        
        public override string Path => $"jwglxt/xtgl/login_slogin.html?time={captcha.timestamp}";
        public override HttpMethod HttpMethod => HttpMethod.Post;

        public LoginRequest(string username, string password, Captcha captcha)
        {
            this.username = username;
            this.password = password;
            this.captcha = captcha;
        }

        public override async Task FillAutoFieldAsync(SimpleServiceContainer serviceContainer)
        {
            await base.FillAutoFieldAsync(serviceContainer);
            var publicKeyResponse = await serviceContainer.Locate<GpnuClient>().SendRequest(new GetLoginPublicKeyRequest(captcha.timestamp));
            if (!publicKeyResponse.IsSucceeded)
                throw new NullReferenceException(publicKeyResponse.message);
            
            var exponent = publicKeyResponse.data.Exponent;
            var modulus = publicKeyResponse.data.Modulus;
            encryptedPassword = EncryptHelper.JwglxtPasswordEncrypt(password, exponent, modulus);
        }

        public override async Task<Response> CreateResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
        {
            var casRequire = responseMessage.RequestMessage!.RequestUri!.Host == "webauth.gpnu.edu.cn";
            if (casRequire)
                return Response.Fail("需要CAS验证");
            var succeeded = responseMessage.RequestMessage!.RequestUri!.AbsolutePath == "/jwglxt/xtgl/index_initMenu.html";
            if (succeeded)
            {
                var req = new HttpRequestMessage(HttpMethod.Get, Host + Path);
                var cache = await PageCache.CreateFromResponseAsync(serviceContainer.Locate<GpnuClient>(), responseMessage, TimeSpan.FromMinutes(10), req);
                serviceContainer.Locate<PageCacheManager>().AddCache(cache);
                return Response.Success();
            }

            var document = await new HtmlParser().ParseDocumentAsync(await responseMessage.Content.ReadAsStreamAsync());
            var tipsElement = document.QuerySelector("#tips");
            return Response.Fail(tipsElement != null ? tipsElement.TextContent.Trim() : "未知的错误");
        }
    }
}
