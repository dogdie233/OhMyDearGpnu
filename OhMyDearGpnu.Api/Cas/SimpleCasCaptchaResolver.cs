using System.Runtime.CompilerServices;

using OhMyDearGpnu.Api.Common.Drawing;

namespace OhMyDearGpnu.Api.Cas;

public class SimpleCasCaptchaResolver : ICasCaptchaResolver
{
    public ValueTask<string> ResolveCaptchaAsync(Image image)
    {
        var a = ParseNumber(image, 0);
        var b = ParseNumber(image, 50);
        var isMultiply = ParseOperator(image);

        var result = isMultiply ? a * b : a + b;
        return ValueTask.FromResult(result.ToString());
    }

    private int ParseNumber(Image image, int xOffset)
    {
        var white = image[0, 0];
        if (GetPixel(1, 6) == white)
        {
            if (GetPixel(2, 5) == white)
                // 4 6
                return GetPixel(3, 5) == white ? 4 : 6;

            // 1 0 5
            if (GetPixel(7, 5) == white)
                return 1;
            return GetPixel(8, 5) == white ? 5 : 0;
        }

        // 2 3 7 8 9
        if (GetPixel(1, 5) == white)
            // 8 9
            return GetPixel(9, 6) == white ? 9 : 8;

        // 2 3 7
        if (GetPixel(8, 5) == white)
            return 2;
        // 3 7
        return GetPixel(9, 6) == white ? 7 : 3;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Rgba GetPixel(int x, int y)
        {
            return image[x + xOffset, y];
        }
    }

    private bool ParseOperator(Image image)
    {
        return image[29, 8] != image[0, 0];
    }
}