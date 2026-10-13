namespace PackageViewer.IO;

/// <summary>
/// Writes (extracts) items of a decrypted package to disk.
/// </summary>
public sealed class Unpacker(Package package) {
  /// <summary>
  /// Extracts all items under outputRoot\&lt;TitleId&gt;\ and returns the output folder
  /// </summary>
  public string ExtractAll(string outputRoot, IProgress<(long done, long total, string name)>? progress = null, CancellationToken token = default) {
    var root = Path.Combine(outputRoot, package.TitleId);
    Directory.CreateDirectory(root);

    var total = (long)package.Files.Where(i => !i.IsDirectory).Sum(i => (decimal)i.DataSize);
    long done = 0;

    foreach (var item in package.Files) {
      token.ThrowIfCancellationRequested();
      var current = item.Name;
      this.Extract(item, root, n => {
        done += n;
        progress?.Report((done, total, current));
      }, token);
    }

    return root;
  }

  /// <summary>
  /// Writes a single file item to destinationPath (a file path)
  /// </summary>
  public void ExtractFile(FileEntry item, string destinationPath, IProgress<(long done, long total, string name)>? progress = null, CancellationToken token = default) {
    if (item.IsDirectory)
      throw new ArgumentException("フォルダは ExtractFolder を使用してください。", nameof(item));

    var total = (long)item.DataSize;
    long done = 0;
    var dir = Path.GetDirectoryName(Path.GetFullPath(destinationPath));
    if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

    using var fs = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
    package.CopyItemTo(item, fs, n => {
      done += n;
      progress?.Report((done, total, item.Name));
    }, token);
  }

  /// <summary>
  /// Extracts a folder item and its contents under outputRoot\&lt;FolderName&gt;\ and returns the output folder
  /// </summary>
  public string ExtractFolder(FileEntry folder, string outputRoot, IProgress<(long done, long total, string name)>? progress = null, CancellationToken token = default) {
    if (!folder.IsDirectory)
      throw new ArgumentException("ファイルは ExtractFile を使用してください。", nameof(folder));

    var folderName = folder.Name.Replace('\\', '/').Trim('/');
    var parent = folderName.Contains('/') ? folderName[..(folderName.LastIndexOf('/') + 1)] : "";
    var prefix = folderName + "/";

    var targets = package.Files
      .Where(i => {
        var n = i.Name.Replace('\\', '/').Trim('/');
        return n.Equals(folderName, StringComparison.OrdinalIgnoreCase) || n.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
      })
      .ToList();

    var total = (long)targets.Where(i => !i.IsDirectory).Sum(i => (decimal)i.DataSize);
    long done = 0;

    foreach (var item in targets) {
      token.ThrowIfCancellationRequested();
      var relative = item.Name.Replace('\\', '/').Trim('/')[parent.Length..]; // Relative path starting from the selected folder name
      var current = item.Name;
      this.Write(item, outputRoot, relative, n => {
        done += n;
        progress?.Report((done, total, current));
      }, token);
    }

    return SafeCombine(outputRoot, folderName[parent.Length..]);
  }

  /// <summary>
  /// Writes a single item to the same relative path under root
  /// </summary>
  public void Extract(FileEntry item, string root, Action<long>? onWritten = null, CancellationToken token = default) {
    this.Write(item, root, item.Name, onWritten, token);
  }

  private void Write(FileEntry item, string root, string relative, Action<long>? onWritten, CancellationToken token) {
    var path = SafeCombine(root, relative);

    if (item.IsDirectory) {
      Directory.CreateDirectory(path);
      return;
    }

    Directory.CreateDirectory(Path.GetDirectoryName(path)!);
    using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
    package.CopyItemTo(item, fs, onWritten, token);
  }

  /// <summary>
  /// Prevents writing outside the output folder (e.g. via "../")
  /// </summary>
  private static string SafeCombine(string root, string relative) {
    var full = Path.GetFullPath(Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar)));
    var rootFull = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
    if (!full.StartsWith(rootFull, StringComparison.OrdinalIgnoreCase))
      throw new InvalidDataException($"不正なパスが含まれています: {relative}");
    return full;
  }
}
