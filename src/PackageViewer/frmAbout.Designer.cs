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
    this.buttonOK = new Button();
    this.label1 = new Label();
    this.label2 = new Label();
    this.SuspendLayout();
    // 
    // buttonOK
    // 
    this.buttonOK.DialogResult = DialogResult.OK;
    this.buttonOK.Location = new Point(203, 109);
    this.buttonOK.Name = "buttonOK";
    this.buttonOK.Size = new Size(75, 23);
    this.buttonOK.TabIndex = 0;
    this.buttonOK.Text = "OK";
    this.buttonOK.UseVisualStyleBackColor = true;
    // 
    // label1
    // 
    this.label1.AutoSize = true;
    this.label1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 128);
    this.label1.Location = new Point(81, 29);
    this.label1.Name = "label1";
    this.label1.Size = new Size(124, 21);
    this.label1.TabIndex = 1;
    this.label1.Text = "Package Viewer";
    // 
    // label2
    // 
    this.label2.AutoSize = true;
    this.label2.Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
    this.label2.Location = new Point(99, 71);
    this.label2.Name = "label2";
    this.label2.Size = new Size(72, 15);
    this.label2.TabIndex = 2;
    this.label2.Text = "Version 1.0.0";
    // 
    // frmAbout
    // 
    this.AutoScaleDimensions = new SizeF(7F, 15F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(290, 144);
    this.Controls.Add(this.label2);
    this.Controls.Add(this.label1);
    this.Controls.Add(this.buttonOK);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.Icon = (Icon)resources.GetObject("$this.Icon");
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = "frmAbout";
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "frmAbout";
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  #endregion

  private Button buttonOK;
  private Label label1;
  private Label label2;
}