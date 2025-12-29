// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using System.Buffers.Binary;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Common.Drawing;

internal class PngReader
{
    private IHDRChunk? _hdr;
    private Rgba[]? _pixels;

    public Image Read(Stream stream)
    {
        _hdr = null;
        _pixels = null;
        try
        {
            using var br = new BinaryReader(stream);
            if (br.ReadUInt32BigEndian() != 0x89504E47 || br.ReadUInt32BigEndian() != 0x0D0A1A0A)
                throw new InvalidImageDataException("Invalid PNG signature");

            ReadChunks(br);
            if (!_hdr.HasValue || _pixels is null)
                throw new InvalidImageDataException("Missing IHDR or IDAT chunk");

            return new Image(_hdr.Value.width, _hdr.Value.height, _pixels);
        }
        catch (Exception e) when (e is not InvalidImageDataException)
        {
            throw new InvalidImageDataException("Unexpected end of stream", e);
        }
    }

    private void ReadChunks(BinaryReader br)
    {
        while (true)
        {
            var length = br.ReadUInt32BigEndian();
            var type = br.ReadUInt32BigEndian();
            switch (type)
            {
                case 0x49484452:
                    ReadIHDRChunk(br, length);
                    break;
                case 0x49444154:
                    ReadIDATChunk(br, length);
                    break;
                case 0x49454E44:
                    ReadIENDChunk(br, length);
                    return;
                default:
                    // Ignore other chunks
                    // Skip data and CRC
                    StreamSkip(br.BaseStream, (int)length + 4);
                    break;
            }
        }
    }

    private void ReadIHDRChunk(BinaryReader br, uint length)
    {
        _hdr = new IHDRChunk
        {
            width = br.ReadInt32BigEndian(),
            height = br.ReadInt32BigEndian(),
            bitDepth = br.ReadByte(),
            colorType = br.ReadByte(),
            compressionMethod = br.ReadByte(),
            filterMethod = br.ReadByte(),
            interlaceMethod = br.ReadByte()
        };
        if (_hdr.Value.colorType != 6)
            throw new InvalidImageDataException("Only truecolor with alpha is supported");
        if (_hdr.Value.compressionMethod != 0 || _hdr.Value.filterMethod != 0 || _hdr.Value.interlaceMethod != 0)
            throw new InvalidImageDataException("Unsupported compression, filter or interlace method");
        // Skip CRC
        StreamSkip(br.BaseStream, 4);
    }

    private void ReadIDATChunk(BinaryReader br, uint length)
    {
        if (!_hdr.HasValue)
            throw new InvalidImageDataException("IDAT chunk must be after IHDR chunk");

        var data = br.ReadBytes((int)length);
        _pixels = DecodeIDATChunkData(_hdr.Value.width, _hdr.Value.height, data);
        // Skip CRC
        StreamSkip(br.BaseStream, 4);
    }

    private void ReadIENDChunk(BinaryReader br, uint length)
    {
        if (_hdr is null || _pixels is null)
            throw new InvalidImageDataException("IEND chunk must be after IHDR and IDAT chunks");
        if (length != 0)
            throw new InvalidImageDataException("IEND chunk must have zero length");
    }

    private void StreamSkip(Stream stream, int count)
    {
        if (stream.CanSeek)
            stream.Seek(count, SeekOrigin.Current);
        else
            stream.ReadExactly(new byte[count], 0, count);
    }

    private record struct IHDRChunk
    {
        public int width;
        public int height;
        public byte bitDepth;
        public byte colorType;
        public byte compressionMethod;
        public byte filterMethod;
        public byte interlaceMethod;
    }

    public static Rgba[] DecodeIDATChunkData(int width, int height, byte[] idatData)
    {
        // 解压缩IDAT数据
        var decompressedData = Decompress(idatData);

        // 解滤波
        var pixel = Unfilter(width, height, decompressedData);

        return pixel;
    }

    private static byte[] Decompress(byte[] data)
    {
        using (var compressedStream = new MemoryStream(data))
        using (var decompressionStream = new ZLibStream(compressedStream, CompressionMode.Decompress))
        using (var resultStream = new MemoryStream())
        {
            decompressionStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }
    }

    private static Rgba[] Unfilter(int width, int height, byte[] data)
    {
        var result = new Rgba[height * width];
        var bytesPerPixel = 4; // RGBA
        var stride = width * bytesPerPixel;
        var viewer = MemoryMarshal.Cast<Rgba, byte>(result.AsSpan());

        var dataIndex = 0;
        for (var y = 0; y < height; y++)
        {
            var filterType = data[dataIndex++];
            for (var x = 0; x < stride; x++)
            {
                var raw = data[dataIndex++];
                var prior = x >= bytesPerPixel ? viewer[y * stride + x - bytesPerPixel] : (byte)0;
                var priorRow = y > 0 ? viewer[(y - 1) * stride + x] : (byte)0;

                viewer[y * stride + x] = filterType switch
                {
                    0 => raw, // None
                    1 => (byte)(raw + prior), // Sub
                    2 => (byte)(raw + priorRow), // Up
                    3 => (byte)(raw + ((prior + priorRow) >> 1)), // Average
                    4 => (byte)(raw + PaethPredictor(prior, priorRow, x >= bytesPerPixel && y > 0 ? viewer[(y - 1) * stride + x - bytesPerPixel] : (byte)0)), // Paeth
                    _ => viewer[y * stride + x] // Invalid
                };
            }
        }

        return result;
    }

    private static byte PaethPredictor(byte a, byte b, byte c)
    {
        var p = a + b - c;
        var pa = Math.Abs(p - a);
        var pb = Math.Abs(p - b);
        var pc = Math.Abs(p - c);

        if (pa <= pb && pa <= pc) return a;
        return pb <= pc ? b : c;
    }
}