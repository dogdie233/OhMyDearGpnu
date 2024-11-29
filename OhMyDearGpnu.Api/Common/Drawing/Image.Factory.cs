namespace OhMyDearGpnu.Api.Common.Drawing;

public partial class Image
{
    public static Image FromSimplePng(byte[] data)
    {
        using var ms = new MemoryStream(data, false);
        return FromSimplePng(ms);
    }

    public static Image FromSimplePng(Stream stream)
    {
        var reader = new PngReader();
        return reader.Read(stream);
    }
}