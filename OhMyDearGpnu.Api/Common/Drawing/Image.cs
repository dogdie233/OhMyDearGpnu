namespace OhMyDearGpnu.Api.Common.Drawing;

public partial class Image
{
    public int Width { get; init; }
    public int Height { get; init; }
    public Rgba[] Pixels { get; init; }

    public Image(int width, int height, Rgba[] pixels)
    {
        if (pixels.Length != width * height)
            throw new ArgumentException("Invalid pixels length");

        Width = width;
        Height = height;
        Pixels = pixels;
    }
}