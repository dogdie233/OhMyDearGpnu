using System.Buffers.Binary;

namespace OhMyDearGpnu.Api.Utility;

public static class BinaryStreamExtension
{
    public static short ReadInt16BigEndian(this BinaryReader binaryReader)
    {
        return BinaryPrimitives.ReadInt16BigEndian(
            binaryReader.BaseStream is MemoryStream ms && ms.TryReadSpanUnsafe(2, out var readBytes) ? readBytes : binaryReader.ReadSpan(stackalloc byte[2]));
    }

    public static ushort ReadUInt16BigEndian(this BinaryReader binaryReader)
    {
        return BinaryPrimitives.ReadUInt16BigEndian(
            binaryReader.BaseStream is MemoryStream ms && ms.TryReadSpanUnsafe(2, out var readBytes) ? readBytes : binaryReader.ReadSpan(stackalloc byte[2]));
    }

    public static int ReadInt32BigEndian(this BinaryReader binaryReader)
    {
        return BinaryPrimitives.ReadInt32BigEndian(
            binaryReader.BaseStream is MemoryStream ms && ms.TryReadSpanUnsafe(4, out var readBytes) ? readBytes : binaryReader.ReadSpan(stackalloc byte[4]));
    }

    public static uint ReadUInt32BigEndian(this BinaryReader binaryReader)
    {
        return BinaryPrimitives.ReadUInt32BigEndian(
            binaryReader.BaseStream is MemoryStream ms && ms.TryReadSpanUnsafe(4, out var readBytes) ? readBytes : binaryReader.ReadSpan(stackalloc byte[4]));
    }

    public static long ReadInt64BigEndian(this BinaryReader binaryReader)
    {
        return BinaryPrimitives.ReadInt64BigEndian(
            binaryReader.BaseStream is MemoryStream ms && ms.TryReadSpanUnsafe(8, out var readBytes) ? readBytes : binaryReader.ReadSpan(stackalloc byte[8]));
    }

    public static ulong ReadUInt64BigEndian(this BinaryReader binaryReader)
    {
        return BinaryPrimitives.ReadUInt64BigEndian(
            binaryReader.BaseStream is MemoryStream ms && ms.TryReadSpanUnsafe(8, out var readBytes) ? readBytes : binaryReader.ReadSpan(stackalloc byte[8]));
    }

    public static Half ReadHalfBigEndian(this BinaryReader binaryReader)
    {
        return BinaryPrimitives.ReadHalfBigEndian(
            binaryReader.BaseStream is MemoryStream ms && ms.TryReadSpanUnsafe(2, out var readBytes) ? readBytes : binaryReader.ReadSpan(stackalloc byte[2]));
    }

    public static float ReadSingleBigEndian(this BinaryReader binaryReader)
    {
        return BinaryPrimitives.ReadSingleBigEndian(
            binaryReader.BaseStream is MemoryStream ms && ms.TryReadSpanUnsafe(4, out var readBytes) ? readBytes : binaryReader.ReadSpan(stackalloc byte[4]));
    }

    public static double ReadDoubleBigEndian(this BinaryReader binaryReader)
    {
        return BinaryPrimitives.ReadDoubleBigEndian(
            binaryReader.BaseStream is MemoryStream ms && ms.TryReadSpanUnsafe(8, out var readBytes) ? readBytes : binaryReader.ReadSpan(stackalloc byte[8]));
    }

    private static ReadOnlySpan<byte> ReadSpan(this BinaryReader binaryReader, Span<byte> buffer)
    {
        binaryReader.BaseStream.ReadExactly(buffer);
        return buffer;
    }

    private static bool TryReadSpanUnsafe(this MemoryStream memoryStream, int numBytes, out ReadOnlySpan<byte> readBytes)
    {
        if (memoryStream.TryGetBuffer(out var msBuffer))
        {
            readBytes = msBuffer.AsSpan((int)memoryStream.Position, numBytes);
            memoryStream.Seek(numBytes, SeekOrigin.Current);
            return true;
        }

        readBytes = [];
        return false;
    }
}