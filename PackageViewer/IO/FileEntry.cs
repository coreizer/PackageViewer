namespace PackageViewer.IO;

/// <summary>
/// A single entry (file or folder) in the file table (ItemRecord + decrypted file name)
/// </summary>
public sealed record FileEntry(string Name, ItemRecord Record) {
  public ulong DataOffset => this.Record.data_offset;
  public ulong DataSize => this.Record.data_size;
  public uint Flags => this.Record.flags;

  /// <summary>
  /// ItemFlag = lower 8 bits of flags (1=NPDRM SELF, 2=EDAT, 3=regular file, 4=folder ...)
  /// </summary>
  public byte ItemFlag => (byte)(this.Record.flags & 0xFF);

  /// <summary>
  /// KeyFlag = (flags &gt;&gt; 0x1C) &amp; 7 (0 = ps3-aes-key, 1 = psp-aes-key)
  /// </summary>
  public int KeyFlag => (int)((this.Record.flags >> 0x1C) & 7);

  public bool IsDirectory => this.ItemFlag == 0x04;

  public string FileName => Path.GetFileName(this.Name);

  public string DirectoryName => Path.GetDirectoryName(this.Name.Replace('\\', '/'))?.Replace('\\', '/') ?? "";

  public FileType GetFileType() {
    if (this.IsDirectory) return FileType.Directory;

    var name = this.FileName.ToUpperInvariant();
    var ext = Path.GetExtension(name);
    if (name == "EBOOT.BIN") return FileType.EBOOT;
    if (name == "PARAM.SFO") return FileType.SFO;
    if (ext is ".SELF" or ".SPRX" || this.ItemFlag == 0x01) return FileType.SELF;
    if (ext is ".PNG" or ".JPG" or ".JPEG" or ".BMP") return FileType.Image;
    if (ext is ".EDAT" || this.ItemFlag == 0x02) return FileType.EDAT; // NPDRM EDAT
    if (ext is ".DAT" or ".SDAT") return FileType.DAT;
    if (ext is ".FF") return FileType.FileFile;
    return FileType.File;
  }
}
