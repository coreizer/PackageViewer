using Avalonia.Media.Imaging;
using Avalonia.Platform;
using PackageViewer.IO;

namespace PackageViewer.Models;

/// <summary>
/// Loads and caches the icons embedded under Assets/Icons
/// </summary>
public static class Icons {
  private static readonly Dictionary<string, Bitmap> _cache = new(StringComparer.OrdinalIgnoreCase);

  public static Bitmap Chip => Get("chip");
  public static Bitmap Data => Get("data");
  public static Bitmap Error => Get("error");
  public static Bitmap Extraction => Get("extraction");
  public static Bitmap File => Get("file");
  public static Bitmap Folder => Get("folder");
  public static Bitmap FolderClose => Get("folder_close");
  public static Bitmap FolderOpen => Get("folder_open");
  public static Bitmap Images => Get("images");
  public static Bitmap Info => Get("info");
  public static Bitmap DriveSearch => Get("drive_search");

  /// <summary>
  /// Returns the icon for the file type of a package entry
  /// </summary>
  public static Bitmap ForEntry(FileEntry item) => item.GetFileType() switch {
    FileType.Directory => FolderClose,
    FileType.Image => Images,
    FileType.EBOOT or FileType.SELF or FileType.SFO or FileType.DAT or FileType.EDAT => Data,
    FileType.File or FileType.FileFile => File,
    _ => Error,
  };

  private static Bitmap Get(string name) {
    lock (_cache) {
      if (_cache.TryGetValue(name, out var bitmap)) return bitmap;
      using var stream = AssetLoader.Open(new Uri($"avares://PackageViewer/Assets/Icons/{name}.png"));
      bitmap = new Bitmap(stream);
      _cache[name] = bitmap;
      return bitmap;
    }
  }
}
