namespace OhMyDearGpnu.Api.Modules.Cas;

public class CasCaptcha
{
    public readonly byte[] image;
    public string? value;

    public CasCaptcha(byte[] image)
    {
        this.image = image;
    }
}
