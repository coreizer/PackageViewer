namespace PackageViewer;

partial class frmAbout {
  /// <summary>
  /// Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  /// Clean up any resources being used.
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
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent() {
    var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
    this.SuspendLayout();
    // 
    // frmAbout
    // 
    this.AutoScaleDimensions = new SizeF(7F, 15F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(479, 351);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.Icon = (Icon)resources.GetObject("$this.Icon");
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = "frmAbout";
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "frmAbout";
    this.ResumeLayout(false);
  }

  #endregion
}