namespace PackageViewer.IO;

/// <summary>
/// Package Item Record
/// </summary>
/// <remarks>
/// All values are in big endian format. <c>Size: 0x20</c>
/// </remarks>
public struct ItemRecord {
  /// <summary>
  /// Filename offset
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x00, Size: 0x04</c>
  /// </remarks>
  public uint filename_offset;

  /// <summary>
  /// Filename length
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x04, Size: 0x04</c>
  /// </remarks>
  public uint filename_size;

  /// <summary>
  /// Data offset
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x08, Size: 0x08</c>
  /// </remarks>
  public ulong data_offset;

  /// <summary>
  /// Data size
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x10, Size: 0x08</c>
  /// </remarks>
  public ulong data_size;

  /// <summary>
  /// The file type. Item_key_type = (flags >> 0x1C)&amp;7 (key type 0 - ps3-aes-key, key type 1 - psp-aes-key)
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x18, Size: 0x04</c>
  /// </remarks>
  public uint flags;

  /// <summary>
  /// zero
  /// </summary>
  /// <remarks>
  /// <c>Offset: 0x1C, Size: 0x04</c>
  /// </remarks>
  public uint padding;
}
