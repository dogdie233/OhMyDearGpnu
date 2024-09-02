using System.Buffers;
using System.Buffers.Text;

namespace OhMyDearGpnu.Api.Utility;

internal static class Utils
{
    public delegate void SplitAction(scoped ReadOnlySpan<char> span);
    
    public static ulong GetCurrentMilliTimestamp() => (ulong)(DateTime.Now - DateTime.UnixEpoch).TotalMilliseconds;

    public static byte[] DecodeBase64(ReadOnlySpan<char> base64)
    {
        var b64Len = base64.Length;
        if (b64Len == 0)
            return [];
        if ((b64Len & 0b11) == 1)
            throw new ArgumentException("Invalid base64 string.");
        var arrLen = ((b64Len >> 2) * 3) + (base64[^1] != '=' ? 1 : 0) + (base64[^2] != '=' ? 1 : 0);
        var arr = new byte[arrLen];
        Convert.TryFromBase64Chars(base64, arr, out _);
        return arr;
    }

    public static void SplitDo(this ReadOnlySpan<char> span, char separator, in SplitAction action)
    {
        var left = 0;
        var right = 0;

        for (; right < span.Length; right++)
        {
            if (span[right] != separator)
                continue;
            if (right - left > 0)
                action(span[left..right]);
            left = right + 1;
        }

        if (right - left > 0)
            action(span[left..right]);
    }
}
