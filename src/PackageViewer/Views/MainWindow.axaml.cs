using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Platform.Storage;
using ByteSizeLib;
using PackageViewer.IO;
using PackageViewer.Models;

namespace PackageViewer.Views;

public partial class MainWindow : Window {
  private Package? _package;
  private CancellationTokenSource? _extractCts;
  private readonly ObservableCollection<FolderNode> _rootNodes = [];
  private readonly ObservableCollection<FileRow> _fileRows = [];
  private readonly Dictionary<string, FolderNode> _folderNodes = new(StringComparer.OrdinalIgnoreCase);

  private static readonly FilePickerFileType PackageFileType = new("Package") { Patterns = ["*.pkg"] };

  public MainWindow() {
    this.InitializeComponent();

    this.folderTreeView.ItemsSource = this._rootNodes;
    this.fileListView.ItemsSource = this._fileRows;

    // Enter on the file list opens a folder / extracts a file (tunnel so the DataGrid does not swallow the key)
    this.fileListView.AddHandler(KeyDownEvent, this.FileListView_KeyDown, RoutingStrategies.Tunnel);

    this.Closed += (_, _) => this.ClosePackage();
    this.UpdateMenuState();
  }

  private void FolderTreeView_SelectionChanged(object? sender, SelectionChangedEventArgs e) {
    try {
      this._fileRows.Clear();
      if (this._package == null) return;
      if (this.folderTreeView.SelectedItem is not FolderNode node) return;

      // Folders first (order within each group follows the PKG)
      foreach (var item in this._package.Files.OrderByDescending(i => i.IsDirectory)) {
        var name = item.Name.Replace('\\', '/').Trim('/');
        var parent = item.IsDirectory
          ? (name.Contains('/') ? name[..name.LastIndexOf('/')] : "")
          : item.DirectoryName;
        if (!string.Equals(parent, node.Path, StringComparison.OrdinalIgnoreCase)) continue;

        this._fileRows.Add(new FileRow(item));
      }
    }
    catch (Exception ex) {
      _ = RuntimeHelpers.ShowErrorAsync(this, "Failed to display the folder contents.", ex);
    }
  }

  private void FileListView_DoubleTapped(object? sender, TappedEventArgs e) {
    // Ignore double-clicks on the column headers
    if (e.Source is Control c && c.FindLogicalAncestorOfType<DataGridColumnHeader>() != null) return;
    this.ActivateSelectedRow();
  }

  private void FileListView_KeyDown(object? sender, KeyEventArgs e) {
    if (e.Key != Key.Enter || this.fileListView.SelectedItem is not FileRow) return;
    this.ActivateSelectedRow();
    e.Handled = true;
  }

  /// <summary>
  /// Double-click / Enter on a row: opens a folder, or extracts a file
  /// </summary>
  private void ActivateSelectedRow() {
    if (this._extractCts != null) return;
    if (this.fileListView.SelectedItem is not FileRow row) return;

    if (row.IsDirectory) {
      this.OpenFolder(row);
    } else {
      _ = this.ExtractRowAsync(row);
    }
  }

  /// <summary>
  /// When a folder is opened in the file list, selects the same folder in the tree and shows its contents
  /// </summary>
  private void OpenFolder(FileRow row) {
    var dir = row.Entry.Name.Replace('\\', '/').Trim('/');
    if (!this._folderNodes.TryGetValue(dir, out var node)) return;

    for (var n = node; n != null; n = n.Parent) n.IsExpanded = true;
    this.folderTreeView.SelectedItem = node; // Triggers FolderTreeView_SelectionChanged
  }

  private void ExitMenuItem_Click(object? sender, RoutedEventArgs e) => this.Close();

  private void CloseMenuItem_Click(object? sender, RoutedEventArgs e) => this.ClosePackage();

  private async void ExtractionMenuItem_Click(object? sender, RoutedEventArgs e) {
    try {
      if (this._package == null) return;

      var outputRoot = await this.PickFolderAsync("Select the output folder");
      if (outputRoot == null) return;

      var unpacker = new Unpacker(this._package);
      await this.RunExtractionAsync((progress, token) => unpacker.ExtractAll(outputRoot, progress, token));
    }
    catch (Exception ex) {
      await RuntimeHelpers.ShowErrorAsync(this, "Extraction failed.", ex);
    }
  }

  /// <summary>
  /// Extracts the item right-clicked in the file list
  /// </summary>
  private async void ExtractMenuItem_Click(object? sender, RoutedEventArgs e) {
    if (this.fileListView.SelectedItem is not FileRow row) return;
    await this.ExtractRowAsync(row);
  }

  /// <summary>
  /// Asks for the destination and extracts a single file or folder row
  /// </summary>
  private async Task ExtractRowAsync(FileRow row) {
    try {
      if (this._package == null || this._extractCts != null) return;
      var item = row.Entry;
      var unpacker = new Unpacker(this._package);

      if (item.IsDirectory) {
        var outputRoot = await this.PickFolderAsync($"Select the output folder for \"{item.FileName}\"");
        if (outputRoot == null) return;

        await this.RunExtractionAsync((progress, token) => unpacker.ExtractFolder(item, outputRoot, progress, token));
      } else {
        var file = await this.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions() {
          Title = "Extract",
          SuggestedFileName = item.FileName,
          ShowOverwritePrompt = true,
          FileTypeChoices = [FilePickerFileTypes.All],
        });
        var path = file?.TryGetLocalPath();
        if (path == null) return;

        await this.RunExtractionAsync((progress, token) => {
          unpacker.ExtractFile(item, path, progress, token);
          return path;
        });
      }
    }
    catch (Exception ex) {
      await RuntimeHelpers.ShowErrorAsync(this, "Extraction failed.", ex);
    }
  }

  private async void AboutMenuItem_Click(object? sender, RoutedEventArgs e) {
    try {
      await new AboutWindow().ShowDialog(this);
    }
    catch (Exception ex) {
      await RuntimeHelpers.ShowErrorAsync(this, "Error", ex);
    }
  }

  private async void OpenMenuItem_Click(object? sender, RoutedEventArgs e) {
    try {
      if (this._extractCts != null) return;

      var files = await this.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions() {
        Title = "Open",
        AllowMultiple = false,
        FileTypeFilter = [PackageFileType, FilePickerFileTypes.All],
      });
      var path = files.Count > 0 ? files[0].TryGetLocalPath() : null;
      if (path == null) return;

      await this.OpenPackageAsync(path);
    }
    catch (Exception ex) {
      await RuntimeHelpers.ShowErrorAsync(this, "Failed to open the package.", ex);
    }
  }

  /// <summary>
  /// Closes the current package and opens the PKG at path (errors are shown in a dialog)
  /// </summary>
  public async Task OpenPackageAsync(string path) {
    this.ClosePackage();

    try {
      this._package = Decoder.Open(path);
    }
    catch (Exception ex) {
      await RuntimeHelpers.ShowErrorAsync(this, "Failed to open the package.", ex);
      return;
    }

    var p = this._package;
    this.Title = $"{RuntimeHelpers.ApplicationName} - {Path.GetFileName(path)}";
    this.statusText.Text =
      $"{p.ContentId}  |  {(p.IsRetail ? "Retail" : "Debug")}  |  {p.Files.Count} items  |  {ByteSize.FromBytes(p.TotalSize).ToString()}";

    this.BuildTree();
    this.UpdateMenuState();
  }

  private void ContextMenu_Opening(object? sender, CancelEventArgs e) {
    e.Cancel = this._package == null || this._extractCts != null || this.fileListView.SelectedItem == null;
  }

  private void ClosePackage() {
    this._extractCts?.Cancel();
    this._package?.Dispose();
    this._package = null;
    this._rootNodes.Clear();
    this._folderNodes.Clear();
    this._fileRows.Clear();
    this.Title = $"{RuntimeHelpers.ApplicationName} - {RuntimeHelpers.Version}";
    this.statusText.Text = "© 2026 coreizer";
    this.UpdateMenuState();
  }

  private void UpdateMenuState() {
    var opened = this._package != null;
    this.closeMenuItem.IsEnabled = opened;
    this.extractionMenuItem.IsEnabled = opened;
  }

  /// <summary>
  /// Builds the folder tree from item paths (fills in missing folder items)
  /// </summary>
  private void BuildTree() {
    if (this._package == null) throw new InvalidOperationException("Package is not opened.");

    this._rootNodes.Clear();
    var nodes = this._folderNodes;
    nodes.Clear();

    var root = new FolderNode(this._package.TitleId, "", null) { IsExpanded = true };
    nodes[""] = root;

    FolderNode Ensure(string dir) {
      if (nodes.TryGetValue(dir, out var n)) return n;
      var slash = dir.LastIndexOf('/');
      var parent = Ensure(slash < 0 ? "" : dir[..slash]);
      n = new FolderNode(slash < 0 ? dir : dir[(slash + 1)..], dir, parent) { IsExpanded = true };
      parent.Children.Add(n);
      nodes[dir] = n;
      return n;
    }

    foreach (var item in this._package.Files) {
      var name = item.Name.Replace('\\', '/').Trim('/');
      Ensure(item.IsDirectory ? name : item.DirectoryName);
    }

    this._rootNodes.Add(root);
    this.folderTreeView.SelectedItem = root;
  }

  /// <summary>
  /// Shows a folder picker and returns the selected local path (null when cancelled)
  /// </summary>
  private async Task<string?> PickFolderAsync(string title) {
    var folders = await this.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions() {
      Title = title,
      AllowMultiple = false,
    });
    return folders.Count > 0 ? folders[0].TryGetLocalPath() : null;
  }

  /// <summary>
  /// Runs extraction in the background and shows progress in the status bar
  /// </summary>
  private async Task RunExtractionAsync(Func<IProgress<(long done, long total, string name)>, CancellationToken, string> extract) {
    this._extractCts = new CancellationTokenSource();
    var token = this._extractCts.Token;
    var status = this.statusText.Text;
    this.mainMenu.IsEnabled = false;

    var progress = new Progress<(long done, long total, string name)>(p => {
      var pct = p.total == 0 ? 100 : p.done * 100 / p.total;
      this.statusText.Text = $"Extracting {pct}% : {p.name}";
    });

    try {
      var output = await Task.Run(() => extract(progress, token));
      this.statusText.Text = status;

      await RuntimeHelpers.ShowInformationAsync(this, "Extraction completed", output);
    }
    catch (OperationCanceledException) {
      this.statusText.Text = status;
    }
    catch (Exception ex) {
      this.statusText.Text = status;
      await RuntimeHelpers.ShowErrorAsync(this, "Extraction failed.", ex);
    }
    finally {
      this.mainMenu.IsEnabled = true;
      this._extractCts?.Dispose();
      this._extractCts = null;
    }
  }
}
