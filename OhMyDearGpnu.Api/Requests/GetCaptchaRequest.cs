using OhMyDearGpnu.Api.Modules;
using OhMyDearGpnu.Api.Responses;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Requests
{
    public class GetCaptchaRequest : BaseWithDataResponseRequest<Captcha>
    {
        public override string Path => $"jwglxt/kaptcha?time={Utils.GetCurrentMilliTimestamp()}";
        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override async Task<DataResponse<Captcha>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
        {
            responseMessage.EnsureSuccessStatusCode();
            MemoryStream? ms = null;
            try
            {
                ms = new MemoryStream((int)(responseMessage.Content.Headers.ContentLength ?? 512));
                await responseMessage.Content.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
            }
            catch
            {
                ms?.Dispose();
                throw;
            }
            var captcha = new Captcha(ms);
            return DataResponse<Captcha>.Success(captcha);
        }
    }
}
