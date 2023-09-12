namespace OhMyDearGpnu.Api.Request
{
    public class LoginRequest : BaseRequest
    {
        [FormItem("yhm")]
        public string username;

        [FormItem("mm")]
        public string password;

        public Captcha captcha;

        public override string Uri => $"jwglxt/xtgl/login_slogin.html?time={captcha.timestamp}";

        public LoginRequest(string username, string password, Captcha captcha)
        {
            this.username = username;
            this.password = password;
            this.captcha = captcha;
        }

        public override IEnumerable<KeyValuePair<string, string>> GetFormItems(PageCacheManager? cachePageManager)
        {
            return base.GetFormItems(cachePageManager);
        }
    }
}
