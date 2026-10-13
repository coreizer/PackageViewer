using System.Buffers.Binary;

namespace PackageViewer.Extensions;

/// <summary>
/// Big-endian read/write helpers for <see cref="Stream"/>
/// </summary>
public static class StreamExtensions {
  public static ushort ReadUInt16BE(this Stream stream) {
    Span<byte> buffer = stackalloc byte[2];
    stream.ReadExactly(buffer);
    return BinaryPrimitives.ReadUInt16BigEndian(buffer);
  }

  public static uint ReadUInt32BE(this Stream stream) {
    Span<byte> buffer = stackalloc byte[4];
    stream.ReadExactly(buffer);
    return BinaryPrimitives.ReadUInt32BigEndian(buffer);
  }

  public static ulong ReadUInt64BE(this Stream stream) {
    Span<byte> buffer = stackalloc byte[8];
    stream.ReadExactly(buffer);
    return BinaryPrimitives.ReadUInt64BigEndian(buffer);
  }

  public static byte[] ReadBytes(this Stream stream, int count) {
    var buffer = new byte[count];
    stream.ReadExactly(buffer);
    return buffer;
  }

  public static void WriteUInt16BE(this Stream stream, ushort value) {
    Span<byte> buffer = stackalloc byte[2];
    BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
    stream.Write(buffer);
  }

  public static void WriteUInt32BE(this Stream stream, uint value) {
    Span<byte> buffer = stackalloc byte[4];
    BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
    stream.Write(buffer);
  }

  public static void WriteUInt64BE(this Stream stream, ulong value) {
    Span<byte> buffer = stackalloc byte[8];
    BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
    stream.Write(buffer);
  }

  /// <summary>
  /// Writes bytes padded with zeros (or truncated) to exactly size bytes
  /// </summary>
  public static void WriteFixed(this Stream stream, ReadOnlySpan<byte> data, int size) {
    Span<byte> buffer = stackalloc byte[size];
    data[..Math.Min(data.Length, size)].CopyTo(buffer);
    stream.Write(buffer);
  }
}
