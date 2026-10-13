using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace PackageViewer.IO;

/// <summary>
/// Decrypts the outer encryption (AES-128-CTR) of a PS3 PKG.
/// Note: EBOOT.BIN / *.self etc. remain NPDRM SELF (only the PKG layer is decrypted).
/// </summary>
public sealed class Decoder : IDisposable {
  /// <summary>
  /// PKG file signature "\x7FPKG" (first 4 bytes of the header, big endian)
  /// </summary>
  public const uint SIGNATURE = 0x7F504B47;

  private static readonly byte[] NPDRM_PKG_PS3_AES_KEY = [
    0x2E, 0x7B, 0x71, 0xD7, 0xC9, 0xC9, 0xA1, 0x4E,
    0xA3, 0x22, 0x1F, 0x18, 0x88, 0x28, 0xB8, 0xF8
  ];

  private const int ITEM_RECORD_SIZE = 0x20;
  private const int CHUNK_SIZE = 1024 * 1024; // Decrypt in 1 MB chunks

  private readonly FileStream _stream;
  private readonly Aes _aes;
  private readonly object _sync = new();
  private readonly byte[] _debugKeyBase = new byte[0x40];

  // Validated (non-null) header values
  private readonly bool _isRetail;
  private readonly ulong _dataOffset;
  private readonly ulong _dataSize;
  private readonly byte[] _dataRiv;

  private Decoder(FileStream stream, PackageHeader header) {
    this._stream = stream;
    this._aes = Aes.Create();
    this._isRetail = (header.pkg_revision & 0x8000) != 0;
    this._dataOffset = header.data_offset;
    this._dataSize = header.data_size;
    this._dataRiv = header.pkg_data_riv;

    if (this._isRetail) {
      this._aes.Key = NPDRM_PKG_PS3_AES_KEY;
    } else {
      // Debug PKG: keystream = SHA1(0x40-byte buffer derived from digest + counter)
      var digest = header.digest;
      Array.Copy(digest, 0, this._debugKeyBase, 0x00, 8);
      Array.Copy(digest, 0, this._debugKeyBase, 0x08, 8);
      Array.Copy(digest, 8, this._debugKeyBase, 0x10, 8);
      Array.Copy(digest, 8, this._debugKeyBase, 0x18, 8);
    }
  }

  /// <summary>
  /// Opens a PKG file, decrypts its header and item table, and returns the package
  /// </summary>
  /// <param name="path">Path to the PKG file</param>
  public static Package Open(string path) {
    var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1 << 16, FileOptions.RandomAccess);
    Decoder? decoder = null;

    try {
      var header = new PackageHeader();
      stream.Seek(0, SeekOrigin.Begin);
      header.Read(stream);
      if (header.magic != SIGNATURE)
        throw new InvalidDataException("Not a PKG file (signature mismatch).");
      if (header.pkg_type != 0x0001)
        throw new NotSupportedException($"Only PS3 PKG files are supported (type: 0x{header.pkg_type:X4}).");

      decoder = new Decoder(stream, header);
      var items = decoder.ReadItems((int)header.item_count);
      return new Package(path, header, items, decoder);
    }
    catch {
      if (decoder != null) {
        decoder.Dispose();
      } else {
        stream.Dispose();
      }
      throw;
    }
  }

  /// <summary>
  /// Returns true if the file starts with the PKG magic
  /// </summary>
  public static bool IsPackage(string path) {
    using var fs = File.OpenRead(path);
    Span<byte> magic = stackalloc byte[4];
    return fs.Read(magic) == 4 && magic.SequenceEqual("\u007FPKG"u8);
  }

  private static ItemRecord ReadItemRecord(ReadOnlySpan<byte> e) => new() {
    filename_offset = BinaryPrimitives.ReadUInt32BigEndian(e[0x00..]),
    filename_size = BinaryPrimitives.ReadUInt32BigEndian(e[0x04..]),
    data_offset = BinaryPrimitives.ReadUInt64BigEndian(e[0x08..]),
    data_size = BinaryPrimitives.ReadUInt64BigEndian(e[0x10..]),
    flags = BinaryPrimitives.ReadUInt32BigEndian(e[0x18..]),
    padding = BinaryPrimitives.ReadUInt32BigEndian(e[0x1C..]),
  };

  private List<FileEntry> ReadItems(int items) {
    var table = this.ReadDecrypted(0, items * ITEM_RECORD_SIZE);
    var list = new List<FileEntry>(items);

    for (var i = 0; i < items; i++) {
      var record = ReadItemRecord(table.AsSpan(i * ITEM_RECORD_SIZE, ITEM_RECORD_SIZE));
      var name = Encoding.UTF8.GetString(this.ReadDecrypted(record.filename_offset, (int)record.filename_size)).TrimEnd('\0');
      list.Add(new FileEntry(name, record));
    }

    return list;
  }

  /// <summary>
  /// Decrypts and returns count bytes starting at relativeOffset within the data area
  /// </summary>
  public byte[] ReadDecrypted(ulong relativeOffset, int count) {
    var buffer = new byte[count];
    this.ReadDecrypted(relativeOffset, buffer);
    return buffer;
  }

  /// <summary>
  /// Decrypts data starting at relativeOffset within the data area into destination
  /// </summary>
  public void ReadDecrypted(ulong relativeOffset, Span<byte> destination) {
    if (relativeOffset + (ulong)destination.Length > this._dataSize)
      throw new InvalidDataException("Attempted to read outside the data area. The PKG may be corrupted.");

    var skip = (int)(relativeOffset % 16);
    var alignedOffset = relativeOffset - (ulong)skip;
    var total = skip + destination.Length;
    var blocks = (total + 15) / 16;

    var buffer = new byte[blocks * 16];
    byte[] keystream;
    lock (this._sync) {
      this._stream.Seek((long)(this._dataOffset + alignedOffset), SeekOrigin.Begin);
      this._stream.ReadAtLeast(buffer, Math.Min(buffer.Length, total), throwOnEndOfStream: true);
      keystream = this.GenerateKeystream(alignedOffset / 16, blocks); // Aes instance is not thread-safe
    }

    for (var i = 0; i < destination.Length; i++)
      destination[i] = (byte)(buffer[skip + i] ^ keystream[skip + i]);
  }

  /// <summary>
  /// Generates keystream for size 16-byte blocks starting at block index
  /// </summary>
  private byte[] GenerateKeystream(ulong index, int size) {
    var buffer = new byte[size * 16];

    if (this._isRetail) {
      // Retail: AES-128-ECB(key, pkg_data_riv + index)  = AES-CTR
      var counter = BinaryPrimitives.ReadUInt128BigEndian(this._dataRiv) + index;
      for (var i = 0; i < size; i++, counter++) {
        BinaryPrimitives.WriteUInt128BigEndian(buffer.AsSpan(i * 16, 16), counter);
      }

      return this._aes.EncryptEcb(buffer, PaddingMode.None);
    }

    // Debug: SHA1(keyBase with counter at 0x38)[0..16]
    var keyBuf = (byte[])this._debugKeyBase.Clone();
    for (var i = 0; i < size; i++) {
      BinaryPrimitives.WriteUInt64BigEndian(keyBuf.AsSpan(0x38), index + (ulong)i);
      SHA1.HashData(keyBuf).AsSpan(0, 16).CopyTo(buffer.AsSpan(i * 16));
    }
    return buffer;
  }

  /// <summary>
  /// Decrypts the item and writes it to output
  /// </summary>
  public void CopyItemTo(FileEntry item, Stream output, Action<long>? onWritten = null, CancellationToken token = default) {
    if (item.IsDirectory) return;

    var buffer = new byte[CHUNK_SIZE];
    ulong done = 0;
    while (done < item.DataSize) {
      token.ThrowIfCancellationRequested();
      var n = (int)Math.Min((ulong)CHUNK_SIZE, item.DataSize - done);
      this.ReadDecrypted(item.DataOffset + done, buffer.AsSpan(0, n));
      output.Write(buffer, 0, n);
      done += (ulong)n;
      onWritten?.Invoke(n);
    }
  }

  public void Dispose() {
    this._stream.Dispose();
    this._aes.Dispose();
  }
}
