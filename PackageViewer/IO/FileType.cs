namespace PackageViewer.IO;

/// <summary>
/// File type of a package entry, determined from its name, extension and item flag
/// </summary>
public enum FileType {

  /// <summary>
  /// EBOOT.BIN (main executable, NPDRM SELF)
  /// </summary>
  EBOOT,

  /// <summary>
  /// PS3 module with the .self / .sprx extension (or item flag 0x01)
  /// </summary>
  SELF,

  /// <summary>
  /// Directory (item flag 0x04)
  /// </summary>
  Directory,

  /// <summary>
  /// Regular file that does not match any other type
  /// </summary>
  File,

  /// <summary>
  /// Image file with extensions such as .png / .jpg / .jpeg / .bmp
  /// </summary>
  Image,

  /// <summary>
  /// File with the .ff extension, included in Call of Duty: Black Ops III packages
  /// </summary>
  FileFile,

  /// <summary>
  /// PARAM.SFO
  /// </summary>
  SFO,

  /// <summary>
  /// Data file with the .dat / .sdat extension
  /// </summary>
  DAT,

  /// <summary>
  /// NPDRM EDAT (.edat extension or item flag 0x02)
  /// </summary>
  EDAT,

  /// <summary>
  /// Unknown file type
  /// </summary>
  Unknown
}
