using PackageViewer.Extensions;
using System.Text;

namespace PackageViewer.IO;

/// <summary>
/// PKG Header
/// </summary>
/// <remarks>
/// All values are in big endian format. <c>Size: 0xC0</c>
/// </remarks>
public class PackageHeader {
  /// <summary>
  /// Header size in bytes
  /// </summary>
  public const int SIZE = 0xC0;

  /// <summary>
  /// ".PKG"
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x00, Size: 0x04</c>
  /// </remarks>
  public uint magic = PackageConstants.SIGNATURE;

  /// <summary>
  /// 80 00 for finalized (retail), 00 00 for non finalized (debug)
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x04, Size: 0x02</c>
  /// </remarks>
  public ushort pkg_revision;

  /// <summary>
  /// 00 01 for PS3, 00 02 for PSP and PS Vita
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x06, Size: 0x02</c>
  /// </remarks>
  public ushort pkg_type;

  /// <summary>
  /// usually 0xC0 for PS3, usually 0x280 for PSP and PS Vita
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x08, Size: 0x04</c>
  /// </remarks>
  public uint pkg_metadata_offset;

  /// <summary>
  /// metadata item count
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x0C, Size: 0x04</c>
  /// </remarks>
  public uint pkg_metadata_count;

  /// <summary>
  /// usually 0xC0 for PSP and PS3, usually 0x160 for PS Vita
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x10, Size: 0x04</c>
  /// </remarks>
  public uint pkg_metadata_size;

  /// <summary>
  /// files and folders into the encrypted data
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x14, Size: 0x04</c>
  /// </remarks>
  public uint item_count;

  /// <summary>
  /// 0x17C0 - total pkg file size
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x18, Size: 0x08</c>
  /// </remarks>
  public ulong total_size;

  /// <summary>
  /// 0x0180 - encrypted data offset
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x20, Size: 0x08</c>
  /// </remarks>
  public ulong data_offset;

  /// <summary>
  /// 0x15E0 - encrypted data size
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x28, Size: 0x08</c>
  /// </remarks>
  public ulong data_size;

  /// <summary>
  /// PKG Content ID
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x30, Size: 0x24</c>
  /// </remarks>
  public string contentid = "";

  /// <summary>
  /// 0x0C bytes padding
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x54, Size: 0x0C</c>
  /// </remarks>
  public byte[] padding = new byte[0x0C];

  /// <summary>
  /// sha1 from debug files and attributes together merged in one block
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x60, Size: 0x10</c>
  /// </remarks>
  public byte[] digest = new byte[0x10];

  /// <summary>
  /// AES-128-CTR IV. Used with npdrm_pkg_PLATFORM_aes_key to decrypt data.
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x70, Size: 0x10</c>
  /// </remarks>
  public byte[] pkg_data_riv = new byte[0x10];

  /// <summary>
  /// PKG Digest structure signing data at 0x00-0x7F.
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x80, Size: 0x40</c>
  /// </remarks>
  public byte[] pkg_header_digest = new byte[0x40];

  /// <summary>
  /// Reads the header (0xC0 bytes) from the current position of the stream.
  /// Byte arrays are kept in file order (not reversed).
  /// </summary>
  public void Read(Stream stream) {
    this.magic = stream.ReadUInt32BE();
    this.pkg_revision = stream.ReadUInt16BE();
    this.pkg_type = stream.ReadUInt16BE();
    this.pkg_metadata_offset = stream.ReadUInt32BE();
    this.pkg_metadata_count = stream.ReadUInt32BE();
    this.pkg_metadata_size = stream.ReadUInt32BE();
    this.item_count = stream.ReadUInt32BE();
    this.total_size = stream.ReadUInt64BE();
    this.data_offset = stream.ReadUInt64BE();
    this.data_size = stream.ReadUInt64BE();
    this.contentid = Encoding.ASCII.GetString(stream.ReadBytes(0x24)).TrimEnd('\0');
    this.padding = stream.ReadBytes(0x0C);
    this.digest = stream.ReadBytes(0x10);
    this.pkg_data_riv = stream.ReadBytes(0x10);
    this.pkg_header_digest = stream.ReadBytes(0x40);
  }
}
