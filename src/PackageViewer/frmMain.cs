using ByteSizeLib;
using PackageViewer.IO;

namespace PackageViewer;

public partial class frmMain : Form {
  private Package? _package;
  private CancellationTokenSource? _extractCts;
  private readonly Dictionary<string, TreeNode> _folderNodes = new(StringComparer.OrdinalIgnoreCase);

  public frmMain() {
    this.InitializeComponent();

    this.UpdateMenuState();
  }

  private void FrmMain_FormClosed(object sender, FormClosedEventArgs e) {
    this.ClosePackage();
  }

  private void FolderTreeView_AfterSelect(object sender, TreeViewEventArgs e) {
    try {
      if (this._package == null) throw new InvalidOperationException("No package is open.");

      this.fileListView.BeginUpdate();
      this.fileListView.Items.Clear();

      // Folders first (order within each group follows the PKG)
      foreach (var item in this._package.Files.OrderByDescending(i => i.IsDirectory)) {
        var name = item.Name.Replace('\\', '/').Trim('/');
        var parent = item.IsDirectory
          ? (name.Contains('/') ? name[..name.LastIndexOf('/')] : "")
          : item.DirectoryName;
        if (!string.Equals(parent, e.Node?.Tag as string ?? "", StringComparison.OrdinalIgnoreCase)) continue;

        var listItem = new ListViewItem(Path.GetFileName(name)) { Tag = item, ImageKey = GetIconKey(item) };
        listItem.SubItems.Add(item.IsDirectory ? "" : ByteSize.FromBytes(item.DataSize).ToString());
        listItem.SubItems.Add(item.GetFileType().ToString());
        listItem.SubItems.Add($"0x{item.DataOffset:X}");
        listItem.SubItems.Add($"0x{item.Flags:X8}");
        this.fileListView.Items.Add(listItem);
      }

      this.fileListView.EndUpdate();
    }
    catch (Exception ex) {
      RuntimeHelpers.ShowError(this, "Failed to display the folder contents.", ex);
    }
  }

  /// <summary>
  /// When a folder is opened in the file list, selects the same folder in the tree and shows its contents
  /// </summary>
  private void FileListView_ItemActivate(object? sender, EventArgs e) {
    if (this.fileListView.SelectedItems.Count == 0) return;
    if (this.fileListView.SelectedItems[0].Tag is not FileEntry item || !item.IsDirectory) return;

    var dir = item.Name.Replace('\\', '/').Trim('/');
    if (!this._folderNodes.TryGetValue(dir, out var node)) return;

    node.Expand();
    this.folderTreeView.SelectedNode = node; // Triggers FolderTreeView_AfterSelect
  }

  private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

  private void CloseToolStripMenuItem_Click(object sender, EventArgs e) => this.ClosePackage();

  private async void ExtractionToolStripMenuItem_Click(object sender, EventArgs e) {
    if (this._package == null) return;

    using var FBD = new FolderBrowserDialog() {
      Description = "Select the output folder",
      UseDescriptionForTitle = true,
    };
    if (FBD.ShowDialog() != DialogResult.OK) return;

    var unpacker = new Unpacker(this._package);
    var outputRoot = FBD.SelectedPath;
    await this.RunExtractionAsync((progress, token) => unpacker.ExtractAll(outputRoot, progress, token));
  }

  /// <summary>
  /// Extracts the item right-clicked in the file list
  /// </summary>
  private async void ExtractToolStripMenuItem_Click(object? sender, EventArgs e) {
    if (this._package == null || this.fileListView.SelectedItems.Count == 0) return;
    if (this.fileListView.SelectedItems[0].Tag is not FileEntry item) return;

    var unpacker = new Unpacker(this._package);

    if (item.IsDirectory) {
      using var FBD = new FolderBrowserDialog() {
        Description = $"Select the output folder for \"{item.FileName}\"",
        UseDescriptionForTitle = true,
      };
      if (FBD.ShowDialog() != DialogResult.OK) return;

      var outputRoot = FBD.SelectedPath;
      await this.RunExtractionAsync((progress, token) => unpacker.ExtractFolder(item, outputRoot, progress, token));
    } else {
      using var SFD = new SaveFileDialog() {
        FileName = item.FileName,
        Filter = "All Files (*.*)|*.*",
        OverwritePrompt = true,
      };
      if (SFD.ShowDialog() != DialogResult.OK) return;

      var path = SFD.FileName;
      await this.RunExtractionAsync((progress, token) => {
        unpacker.ExtractFile(item, path, progress, token);
        return path;
      });
    }
  }

  private void AboutToolStripMenuItem_Click(object sender, EventArgs e) {
    try {
      using var frm = new frmAbout();
      frm.ShowDialog();
    }
    catch (Exception ex) {
      RuntimeHelpers.ShowError(this, "Error", ex);
    }
  }

  private void OpenToolStripMenuItem_Click(object sender, EventArgs e) {
    try {
      using var OFD = new OpenFileDialog() {
        AddExtension = true,
        Filter = "Package (*.pkg)|*.pkg|All Files (*.*)|*.*",
      };
      if (OFD.ShowDialog() != DialogResult.OK) return;

      this.ClosePackage();

      try {
        this._package = Decoder.Open(OFD.FileName);
      }
      catch (Exception ex) {
        RuntimeHelpers.ShowError(this, "Failed to open the package.", ex);
        return;
      }

      var p = this._package;
      this.Text = $"{RuntimeHelpers.ApplicationName} - {Path.GetFileName(OFD.FileName)}";
      this.toolStripStatusLabel.Text =
        $"{p.ContentId}  |  {(p.IsRetail ? "Retail" : "Debug")}  |  {p.Files.Count} items  |  {ByteSize.FromBytes(p.TotalSize).ToString()}";

      this.BuildTree();
      this.UpdateMenuState();
    }
    catch (Exception ex) {
      RuntimeHelpers.ShowError(this, "Failed to open the package.", ex);
    }
  }

  private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
    e.Cancel = this._package == null || this._extractCts != null || this.fileListView.SelectedItems.Count == 0;
  }

  private void ClosePackage() {
    this._extractCts?.Cancel();
    this._package?.Dispose();
    this._package = null;
    this.folderTreeView.Nodes.Clear();
    this._folderNodes.Clear();
    this.fileListView.Items.Clear();
    this.Text = "PS3 Package Viewer - 1.0.0";
    this.toolStripStatusLabel.Text = "© 2026 coreizer";
    this.UpdateMenuState();
  }

  private void UpdateMenuState() {
    var opened = this._package != null;
    this.closeToolStripMenuItem.Enabled = opened;
    this.extractionToolStripMenuItem.Enabled = opened;
  }

  /// <summary>
  /// Builds the folder tree from item paths (fills in missing folder items)
  /// </summary>
  private void BuildTree() {
    if (this._package == null) throw new InvalidOperationException("Package is not opened.");

    this.folderTreeView.BeginUpdate();
    this.folderTreeView.Nodes.Clear();

    var root = new TreeNode(this._package.TitleId) { Tag = "" };
    this.folderTreeView.Nodes.Add(root);
    var nodes = this._folderNodes;
    nodes.Clear();
    nodes[""] = root;

    TreeNode Ensure(string dir) {
      if (nodes.TryGetValue(dir, out var n)) return n;
      var slash = dir.LastIndexOf('/');
      var parent = Ensure(slash < 0 ? "" : dir[..slash]);
      n = new TreeNode(slash < 0 ? dir : dir[(slash + 1)..]) { Tag = dir };
      parent.Nodes.Add(n);
      nodes[dir] = n;
      return n;
    }

    foreach (var item in this._package.Files) {
      var name = item.Name.Replace('\\', '/').Trim('/');
      Ensure(item.IsDirectory ? name : item.DirectoryName);
    }

    root.ExpandAll();
    this.folderTreeView.EndUpdate();
    this.folderTreeView.SelectedNode = root;
  }

  /// <summary>
  /// Runs extraction in the background and shows progress in the status bar
  /// </summary>
  private async Task RunExtractionAsync(Func<IProgress<(long done, long total, string name)>, CancellationToken, string> extract) {
    this._extractCts = new CancellationTokenSource();
    var token = this._extractCts.Token;
    var status = this.toolStripStatusLabel.Text;
    this.menuStrip.Enabled = false;

    var progress = new Progress<(long done, long total, string name)>(p => {
      var pct = p.total == 0 ? 100 : p.done * 100 / p.total;
      this.toolStripStatusLabel.Text = $"Extracting {pct}% : {p.name}";
    });

    try {
      var output = await Task.Run(() => extract(progress, token));
      this.toolStripStatusLabel.Text = status;

      TaskDialog.ShowDialog(this, new TaskDialogPage() {
        Caption = RuntimeHelpers.ApplicationName,
        Heading = "Extraction completed",
        Text = output,
        Icon = TaskDialogIcon.Information,
        Buttons = { TaskDialogButton.OK },
      });
    }
    catch (OperationCanceledException) {
      this.toolStripStatusLabel.Text = status;
    }
    catch (Exception ex) {
      this.toolStripStatusLabel.Text = status;
      RuntimeHelpers.ShowError(this, "Extraction failed.", ex);
    }
    finally {
      this.menuStrip.Enabled = true;
      this._extractCts?.Dispose();
      this._extractCts = null;
    }
  }

  /// <summary>
  /// Returns the iconImageList key for the file type
  /// </summary>
  private static string GetIconKey(FileEntry item) => item.GetFileType() switch {
    FileType.Directory => "folder_close.ico",
    FileType.Image => "images.ico",
    FileType.EBOOT or FileType.SELF or FileType.SFO or FileType.DAT or FileType.EDAT => "data.ico",
    FileType.File or FileType.FileFile => "file.ico",
    _ => "error.ico",
  };
}
