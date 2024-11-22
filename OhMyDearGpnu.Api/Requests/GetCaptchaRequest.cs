using OhMyDearGpnu.Api.Modules;
using OhMyDearGpnu.Api.Responses;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Requests;

[Request(PayloadTypeEnum.None)]
public partial class GetCaptchaRequest : BaseWithDataResponseRequest<Captcha>
{
    private readonly ulong timestamp = Utils.GetCurrentMilliTimestamp();

    public override Uri Url => new(Hosts.acaAff, $"jwglxt/kaptcha?time={timestamp}");
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

        var captcha = new Captcha(timestamp, ms);
        return DataResponse<Captcha>.Success(captcha);
    }
}