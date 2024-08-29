namespace OhMyDearGpnu.Api.Modules.Cas;

public class CasCaptcha
{
    public readonly Guid uid;
    public readonly byte[] image;
    public readonly DateTime expireAt;
    public string? value;

    public CasCaptcha(Guid uid, byte[] image, DateTime expireAt)
    {
        this.uid = uid;
        this.image = image;
        this.expireAt = expireAt;
    }
}
