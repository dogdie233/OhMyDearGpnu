using AngleSharp.Html.Parser;
using OhMyDearGpnu.Api.Modules;
using OhMyDearGpnu.Api.Responses;

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

        public override string Path => $"/jwglxt/xtgl/login_slogin.html?time={Utils.GetCurrentMilliTimestamp()}";
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
            var publicKeyResponse = await serviceContainer.Locate<GpnuClient>().SendRequest(new GetLoginPublicKeyRequest(Utils.GetCurrentMilliTimestamp()));
            if (!publicKeyResponse.IsSucceeded)
                throw new NullReferenceException(publicKeyResponse.message);

            encryptedPassword = EncryptHelper.Encrypt(password, publicKeyResponse.data!.Exponent, publicKeyResponse.data!.Modulus);
        }

        public override async Task<Response> CreateResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
        {
            var succeeded = responseMessage.RequestMessage!.RequestUri!.AbsolutePath == "/jwglxt/xtgl/index_initMenu.html";
            if (succeeded)
            {
                var req = new HttpRequestMessage(HttpMethod.Get, Path);
                var cache = await PageCache.CreateFromResponseAsync(serviceContainer.Locate<GpnuClient>(), responseMessage, TimeSpan.FromMinutes(10), req);
                serviceContainer.Locate<PageCacheManager>().AddCache(cache);
                return new Response(null);
            }

            var document = await new HtmlParser().ParseDocumentAsync(responseMessage.Content.ReadAsStream());
            var tipsElement = document.QuerySelector("#tips");
            if (tipsElement != null)
                return new Response(tipsElement.TextContent.Trim());
            return new Response("未知的错误");
        }
    }
}
