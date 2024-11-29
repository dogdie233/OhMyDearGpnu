using System.Runtime.InteropServices;

namespace OhMyDearGpnu.Api.Common.Drawing;

[StructLayout(LayoutKind.Explicit)]
public struct Rgba : IEquatable<Rgba>, IEquatable<Rgba?>
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

    public bool Equals(Rgba other)
    {
        return value == other.value;
    }

    public bool Equals(Rgba? other)
    {
        return other.HasValue && Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Rgba other && Equals(other);
    }

    public override int GetHashCode()
    {
        return (int)value;
    }

    public static bool operator ==(Rgba left, Rgba right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rgba left, Rgba right)
    {
        return !left.Equals(right);
    }
}