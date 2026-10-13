namespace PackageViewer;

partial class frmMain {
  /// <summary>
  ///  Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  ///  Clean up any resources being used.
  /// </summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing) {
    if (disposing && (components != null)) {
      components.Dispose();
    }
    base.Dispose(disposing);
  }

  #region Windows Form Designer generated code

  /// <summary>
  ///  Required method for Designer support - do not modify
  ///  the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent() {
    this.components = new System.ComponentModel.Container();
    var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
    this.menuStrip = new MenuStrip();
    this.fileToolStripMenuItem = new ToolStripMenuItem();
    this.toolStripMenuItem = new ToolStripMenuItem();
    this.openToolStripMenuItem = new ToolStripMenuItem();
    this.file1ToolStripSeparator = new ToolStripSeparator();
    this.closeToolStripMenuItem = new ToolStripMenuItem();
    this.file2ToolStripSeparator = new ToolStripSeparator();
    this.saveAsToolStripMenuItem = new ToolStripMenuItem();
    this.file3ToolStripSeparator = new ToolStripSeparator();
    this.exitToolStripMenuItem = new ToolStripMenuItem();
    this.toolsToolStripMenuItem = new ToolStripMenuItem();
    this.extractionToolStripMenuItem = new ToolStripMenuItem();
    this.helpToolStripMenuItem = new ToolStripMenuItem();
    this.aboutToolStripMenuItem = new ToolStripMenuItem();
    this.statusStrip = new StatusStrip();
    this.toolStripStatusLabel = new ToolStripStatusLabel();
    this.splitContainer = new SplitContainer();
    this.folderTreeView = new TreeView();
    this.iconImageList = new ImageList(this.components);
    this.fileListView = new ListView();
    this.fileNameColumnHeader = new ColumnHeader();
    this.fileSizeClumnHeader = new ColumnHeader();
    this.fileTypeColumnHeader = new ColumnHeader();
    this.fileOffsetColumnHeader = new ColumnHeader();
    this.fileFlagColumnHeader = new ColumnHeader();
    this.contextMenuStrip = new ContextMenuStrip(this.components);
    this.toolStripMenuItemExtract = new ToolStripMenuItem();
    this.menuStrip.SuspendLayout();
    this.statusStrip.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)this.splitContainer).BeginInit();
    this.splitContainer.Panel1.SuspendLayout();
    this.splitContainer.Panel2.SuspendLayout();
    this.splitContainer.SuspendLayout();
    this.contextMenuStrip.SuspendLayout();
    this.SuspendLayout();
    // 
    // menuStrip
    // 
    this.menuStrip.Items.AddRange(new ToolStripItem[] { this.fileToolStripMenuItem, this.toolsToolStripMenuItem, this.helpToolStripMenuItem });
    this.menuStrip.Location = new Point(0, 0);
    this.menuStrip.Name = "menuStrip";
    this.menuStrip.Size = new Size(959, 24);
    this.menuStrip.TabIndex = 0;
    this.menuStrip.Text = "menuStrip1";
    // 
    // fileToolStripMenuItem
    // 
    this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.toolStripMenuItem, this.openToolStripMenuItem, this.file1ToolStripSeparator, this.closeToolStripMenuItem, this.file2ToolStripSeparator, this.saveAsToolStripMenuItem, this.file3ToolStripSeparator, this.exitToolStripMenuItem });
    this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
    this.fileToolStripMenuItem.Size = new Size(37, 20);
    this.fileToolStripMenuItem.Text = "File";
    // 
    // toolStripMenuItem
    // 
    this.toolStripMenuItem.Enabled = false;
    this.toolStripMenuItem.Name = "toolStripMenuItem";
    this.toolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
    this.toolStripMenuItem.Size = new Size(180, 22);
    this.toolStripMenuItem.Text = "New";
    // 
    // openToolStripMenuItem
    // 
    this.openToolStripMenuItem.Name = "openToolStripMenuItem";
    this.openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
    this.openToolStripMenuItem.Size = new Size(180, 22);
    this.openToolStripMenuItem.Text = "Open";
    this.openToolStripMenuItem.Click += this.OpenToolStripMenuItem_Click;
    // 
    // file1ToolStripSeparator
    // 
    this.file1ToolStripSeparator.Name = "file1ToolStripSeparator";
    this.file1ToolStripSeparator.Size = new Size(177, 6);
    // 
    // closeToolStripMenuItem
    // 
    this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
    this.closeToolStripMenuItem.Size = new Size(180, 22);
    this.closeToolStripMenuItem.Text = "Close";
    this.closeToolStripMenuItem.Click += this.CloseToolStripMenuItem_Click;
    // 
    // file2ToolStripSeparator
    // 
    this.file2ToolStripSeparator.Name = "file2ToolStripSeparator";
    this.file2ToolStripSeparator.Size = new Size(177, 6);
    // 
    // saveAsToolStripMenuItem
    // 
    this.saveAsToolStripMenuItem.Enabled = false;
    this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
    this.saveAsToolStripMenuItem.Size = new Size(180, 22);
    this.saveAsToolStripMenuItem.Text = "Save as";
    // 
    // file3ToolStripSeparator
    // 
    this.file3ToolStripSeparator.Name = "file3ToolStripSeparator";
    this.file3ToolStripSeparator.Size = new Size(177, 6);
    // 
    // exitToolStripMenuItem
    // 
    this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
    this.exitToolStripMenuItem.Size = new Size(180, 22);
    this.exitToolStripMenuItem.Text = "Exit";
    this.exitToolStripMenuItem.Click += this.ExitToolStripMenuItem_Click;
    // 
    // toolsToolStripMenuItem
    // 
    this.toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.extractionToolStripMenuItem });
    this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
    this.toolsToolStripMenuItem.Size = new Size(46, 20);
    this.toolsToolStripMenuItem.Text = "Tools";
    // 
    // extractionToolStripMenuItem
    // 
    this.extractionToolStripMenuItem.Name = "extractionToolStripMenuItem";
    this.extractionToolStripMenuItem.Size = new Size(180, 22);
    this.extractionToolStripMenuItem.Text = "Extraction";
    this.extractionToolStripMenuItem.Click += this.ExtractionToolStripMenuItem_Click;
    // 
    // helpToolStripMenuItem
    // 
    this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.aboutToolStripMenuItem });
    this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
    this.helpToolStripMenuItem.Size = new Size(44, 20);
    this.helpToolStripMenuItem.Text = "Help";
    // 
    // aboutToolStripMenuItem
    // 
    this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
    this.aboutToolStripMenuItem.Size = new Size(180, 22);
    this.aboutToolStripMenuItem.Text = "About";
    this.aboutToolStripMenuItem.Click += this.AboutToolStripMenuItem_Click;
    // 
    // statusStrip
    // 
    this.statusStrip.Items.AddRange(new ToolStripItem[] { this.toolStripStatusLabel });
    this.statusStrip.Location = new Point(0, 507);
    this.statusStrip.Name = "statusStrip";
    this.statusStrip.Size = new Size(959, 22);
    this.statusStrip.TabIndex = 1;
    this.statusStrip.Text = "statusStrip1";
    // 
    // toolStripStatusLabel
    // 
    this.toolStripStatusLabel.Name = "toolStripStatusLabel";
    this.toolStripStatusLabel.Size = new Size(89, 17);
    this.toolStripStatusLabel.Text = "© 2026 coreizer";
    // 
    // splitContainer
    // 
    this.splitContainer.Dock = DockStyle.Fill;
    this.splitContainer.Location = new Point(0, 24);
    this.splitContainer.Name = "splitContainer";
    // 
    // splitContainer.Panel1
    // 
    this.splitContainer.Panel1.Controls.Add(this.folderTreeView);
    // 
    // splitContainer.Panel2
    // 
    this.splitContainer.Panel2.Controls.Add(this.fileListView);
    this.splitContainer.Size = new Size(959, 483);
    this.splitContainer.SplitterDistance = 318;
    this.splitContainer.TabIndex = 2;
    // 
    // folderTreeView
    // 
    this.folderTreeView.Dock = DockStyle.Fill;
    this.folderTreeView.HideSelection = false;
    this.folderTreeView.ImageIndex = 3;
    this.folderTreeView.ImageList = this.iconImageList;
    this.folderTreeView.Location = new Point(0, 0);
    this.folderTreeView.Name = "folderTreeView";
    this.folderTreeView.SelectedImageIndex = 4;
    this.folderTreeView.Size = new Size(318, 483);
    this.folderTreeView.TabIndex = 0;
    this.folderTreeView.AfterSelect += this.FolderTreeView_AfterSelect;
    // 
    // iconImageList
    // 
    this.iconImageList.ColorDepth = ColorDepth.Depth32Bit;
    this.iconImageList.ImageStream = (ImageListStreamer)resources.GetObject("iconImageList.ImageStream");
    this.iconImageList.TransparentColor = Color.Transparent;
    this.iconImageList.Images.SetKeyName(0, "data.ico");
    this.iconImageList.Images.SetKeyName(1, "error.ico");
    this.iconImageList.Images.SetKeyName(2, "file.ico");
    this.iconImageList.Images.SetKeyName(3, "folder_close.ico");
    this.iconImageList.Images.SetKeyName(4, "folder_open.ico");
    this.iconImageList.Images.SetKeyName(5, "images.ico");
    // 
    // fileListView
    // 
    this.fileListView.Columns.AddRange(new ColumnHeader[] { this.fileNameColumnHeader, this.fileSizeClumnHeader, this.fileTypeColumnHeader, this.fileOffsetColumnHeader, this.fileFlagColumnHeader });
    this.fileListView.ContextMenuStrip = this.contextMenuStrip;
    this.fileListView.Dock = DockStyle.Fill;
    this.fileListView.FullRowSelect = true;
    this.fileListView.LargeImageList = this.iconImageList;
    this.fileListView.Location = new Point(0, 0);
    this.fileListView.MultiSelect = false;
    this.fileListView.Name = "fileListView";
    this.fileListView.Size = new Size(637, 483);
    this.fileListView.SmallImageList = this.iconImageList;
    this.fileListView.TabIndex = 0;
    this.fileListView.UseCompatibleStateImageBehavior = false;
    this.fileListView.View = View.Details;
    this.fileListView.ItemActivate += this.FileListView_ItemActivate;
    // 
    // fileNameColumnHeader
    // 
    this.fileNameColumnHeader.Text = "Name";
    this.fileNameColumnHeader.Width = 220;
    // 
    // fileSizeClumnHeader
    // 
    this.fileSizeClumnHeader.Text = "Size";
    this.fileSizeClumnHeader.TextAlign = HorizontalAlignment.Right;
    this.fileSizeClumnHeader.Width = 100;
    // 
    // fileTypeColumnHeader
    // 
    this.fileTypeColumnHeader.Text = "Type";
    this.fileTypeColumnHeader.Width = 70;
    // 
    // fileOffsetColumnHeader
    // 
    this.fileOffsetColumnHeader.Text = "Offset";
    this.fileOffsetColumnHeader.Width = 100;
    // 
    // fileFlagColumnHeader
    // 
    this.fileFlagColumnHeader.Text = "Flag";
    this.fileFlagColumnHeader.Width = 90;
    // 
    // contextMenuStrip
    // 
    this.contextMenuStrip.Items.AddRange(new ToolStripItem[] { this.toolStripMenuItemExtract });
    this.contextMenuStrip.Name = "contextMenuStrip1";
    this.contextMenuStrip.Size = new Size(120, 26);
    this.contextMenuStrip.Opening += this.ContextMenuStrip_Opening;
    // 
    // toolStripMenuItemExtract
    // 
    this.toolStripMenuItemExtract.Name = "toolStripMenuItemExtract";
    this.toolStripMenuItemExtract.Size = new Size(119, 22);
    this.toolStripMenuItemExtract.Text = "Extract...";
    this.toolStripMenuItemExtract.Click += this.ExtractToolStripMenuItem_Click;
    // 
    // frmMain
    // 
    this.AutoScaleDimensions = new SizeF(7F, 15F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(959, 529);
    this.Controls.Add(this.splitContainer);
    this.Controls.Add(this.statusStrip);
    this.Controls.Add(this.menuStrip);
    this.Icon = (Icon)resources.GetObject("$this.Icon");
    this.MainMenuStrip = this.menuStrip;
    this.Name = "frmMain";
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "PS3 Package Viewer - 1.0.0";
    this.FormClosed += this.FrmMain_FormClosed;
    this.menuStrip.ResumeLayout(false);
    this.menuStrip.PerformLayout();
    this.statusStrip.ResumeLayout(false);
    this.statusStrip.PerformLayout();
    this.splitContainer.Panel1.ResumeLayout(false);
    this.splitContainer.Panel2.ResumeLayout(false);
    ((System.ComponentModel.ISupportInitialize)this.splitContainer).EndInit();
    this.splitContainer.ResumeLayout(false);
    this.contextMenuStrip.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  #endregion

  private MenuStrip menuStrip;
  private ToolStripMenuItem fileToolStripMenuItem;
  private ToolStripMenuItem openToolStripMenuItem;
  private ToolStripMenuItem closeToolStripMenuItem;
  private ToolStripMenuItem toolsToolStripMenuItem;
  private ToolStripMenuItem extractionToolStripMenuItem;
  private ToolStripMenuItem helpToolStripMenuItem;
  private ToolStripMenuItem aboutToolStripMenuItem;
  private StatusStrip statusStrip;
  private ToolStripStatusLabel toolStripStatusLabel;
  private SplitContainer splitContainer;
  private TreeView folderTreeView;
  private ToolStripMenuItem toolStripMenuItem;
  private ToolStripSeparator file1ToolStripSeparator;
  private ToolStripSeparator file2ToolStripSeparator;
  private ToolStripMenuItem saveAsToolStripMenuItem;
  private ToolStripSeparator file3ToolStripSeparator;
  private ToolStripMenuItem exitToolStripMenuItem;
  private ListView fileListView;
  private ImageList iconImageList;
  private ColumnHeader fileNameColumnHeader;
  private ColumnHeader fileSizeClumnHeader;
  private ColumnHeader fileTypeColumnHeader;
  private ColumnHeader fileOffsetColumnHeader;
  private ColumnHeader fileFlagColumnHeader;
  private ContextMenuStrip contextMenuStrip;
  private ToolStripMenuItem toolStripMenuItemExtract;
}
