namespace PackageViewer;

public partial class frmAbout : Form {
  public frmAbout() {
    this.InitializeComponent();
    this.Text = $"About - {RuntimeHelpers.ApplicationName}";
  }
}
