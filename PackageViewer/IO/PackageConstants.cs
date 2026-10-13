namespace PackageViewer.IO;

/// <summary>
/// Constants for the PS3 PKG format
/// </summary>
public static class PackageConstants {
  /// <summary>
  /// PKG file signature "\x7FPKG" (first 4 bytes of the header, big endian)
  /// </summary>
  public const uint SIGNATURE = 0x7F504B47;
}
