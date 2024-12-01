namespace OhMyDearGpnu.Api.AcaAff;

public class Captcha(ulong timestamp, byte[] image)
{
    public readonly ulong timestamp = timestamp;
    public readonly byte[] image = image;
    public string? value;
}