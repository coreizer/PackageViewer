using Avalonia.Media.Imaging;
using ByteSizeLib;
using PackageViewer.IO;

namespace PackageViewer.Models;

/// <summary>
/// A row in the file list (one package entry)
/// </summary>
public sealed class FileRow {
  public FileRow(FileEntry entry) {
    this.Entry = entry;
    var name = entry.Name.Replace('\\', '/').Trim('/');
    this.Name = System.IO.Path.GetFileName(name);
    this.Size = entry.IsDirectory ? "" : ByteSize.FromBytes(entry.DataSize).ToString();
    this.Type = entry.GetFileType().ToString();
    this.Offset = $"0x{entry.DataOffset:X}";
    this.Flag = $"0x{entry.Flags:X8}";
    this.Icon = Icons.ForEntry(entry);
  }

  public FileEntry Entry { get; }

  public bool IsDirectory => this.Entry.IsDirectory;

  public string Name { get; }

  public string Size { get; }

  public string Type { get; }

  public string Offset { get; }

  public string Flag { get; }

  public Bitmap Icon { get; }
}
