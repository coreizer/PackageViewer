using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PackageViewer.Views;

public partial class AboutWindow : Window {
  public AboutWindow() {
    this.InitializeComponent();
    this.Title = $"About - {RuntimeHelpers.ApplicationName}";
    this.versionText.Text = $"Version {RuntimeHelpers.Version}";
  }

  private void OkButton_Click(object? sender, RoutedEventArgs e) => this.Close();
}
