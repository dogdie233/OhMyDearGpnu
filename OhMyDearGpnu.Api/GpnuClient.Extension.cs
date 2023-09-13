using OhMyDearGpnu.Api.Requests;
using OhMyDearGpnu.Api.Responses;

namespace OhMyDearGpnu.Api
{
    public static class GpnuClientExtension
    {
        public static Task<Response> Login(this GpnuClient gpnuClient, string username, string password, Captcha captcha)
            => gpnuClient.SendRequest(new LoginRequest(username, password, captcha));

        public static Task<DataResponse<Captcha>> GetCaptcha(this GpnuClient gpnuClient, string? timestamp = null)
            => gpnuClient.SendRequest(new GetCaptchaRequest(timestamp));
    }
}
