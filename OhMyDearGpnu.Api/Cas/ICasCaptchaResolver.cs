using OhMyDearGpnu.Api.Common.Drawing;

namespace OhMyDearGpnu.Api.Cas;

public interface ICasCaptchaResolver
{
    public ValueTask<string> ResolveCaptchaAsync(Image image);
}