using System.Runtime.InteropServices;

namespace OhMyDearGpnu.Api.Common.Drawing;

[StructLayout(LayoutKind.Explicit)]
public record struct Rgba
{
    [FieldOffset(0)] public byte r;
    [FieldOffset(1)] public byte g;
    [FieldOffset(2)] public byte b;
    [FieldOffset(3)] public byte a;

    [FieldOffset(0)] public uint value;

    public Rgba(byte r, byte g, byte b, byte a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Rgba(uint value)
    {
        this.value = value;
    }
}