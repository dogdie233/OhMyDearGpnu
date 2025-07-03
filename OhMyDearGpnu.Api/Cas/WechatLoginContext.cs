using OhMyDearGpnu.Api.Cas.Requests;
using OhMyDearGpnu.Api.Cas.Responses;
using OhMyDearGpnu.Api.Common.Drawing;

namespace OhMyDearGpnu.Api.Cas;

public class WechatLoginContext
{
    public string Uuid { get; init; }
    public DateTime StartAt { get; init; }
    public byte[] QrCodeImageData { get; init; }
    public Lazy<Image> QrCodeImage { get; init; }
    

    internal WechatLoginContext(string uuid, byte[] qrCodeImageData)
    {
        Uuid = uuid;
        StartAt = DateTime.Now;
        QrCodeImageData = qrCodeImageData;
        QrCodeImage = new Lazy<Image>(() => Image.FromSimplePng(QrCodeImageData));
    }
}