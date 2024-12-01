using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.AcaAff.Requests;

[Request(PayloadTypeEnum.None)]
public partial class GetCaptchaRequest : BaseRequest<Captcha>
{
    private readonly ulong timestamp = Utils.GetCurrentMilliTimestamp();

    public override Uri Url => new(Hosts.acaAff, $"jwglxt/kaptcha?time={timestamp}");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override async ValueTask<Captcha> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        using var ms = new MemoryStream((int)(responseMessage.Content.Headers.ContentLength ?? 512));
        await responseMessage.Content.CopyToAsync(ms);
        return new Captcha(timestamp, ms.ToArray());
    }
}