using PackageViewer.IO;

namespace PackageViewer;

/// <summary>
/// A decrypted PS3 PKG (header, item list and package information).
/// Created by <see cref="Decoder.Open(string)"/>.
/// </summary>
public sealed class Package : IDisposable {
  private readonly Decoder _decoder;

  internal Package(string filePath, PackageHeader header, IReadOnlyList<FileEntry> items, Decoder decoder) {
    this.FilePath = filePath;
    this.Header = header;
    this.Files = items;
    this._decoder = decoder;
  }

  public string FilePath { get; }

  public PackageHeader Header { get; }

  public IReadOnlyList<FileEntry> Files { get; }

  public bool IsRetail => (this.Header.pkg_revision & 0x8000) != 0;

  public string ContentId => this.Header.contentid;

  public ulong TotalSize => this.Header.total_size;

  /// <summary>
  /// "UP0002-BLUS30377_00-..." → "BLUS30377"
  /// </summary>
  public string TitleId {
    get {
      var parts = this.ContentId.Split('-');
      return parts.Length > 1 ? parts[1].Split('_')[0] : this.ContentId;
    }
  }

  /// <summary>
  /// Decrypts the item and writes it to output
  /// </summary>
  public void CopyItemTo(FileEntry item, Stream output, Action<long>? onWritten = null, CancellationToken token = default) {
    this._decoder.CopyItemTo(item, output, onWritten, token);
  }

  /// <summary>
  /// Decrypts the item and returns its contents
  /// </summary>
  public byte[] ReadItem(FileEntry item) {
    if (item.DataSize > int.MaxValue) throw new IOException("The file is too large.");
    using var ms = new MemoryStream((int)item.DataSize);
    this.CopyItemTo(item, ms);
    return ms.ToArray();
  }

  public void Dispose() {
    this._decoder.Dispose();
  }
}
