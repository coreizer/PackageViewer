using Avalonia.Controls;
using Avalonia.Interactivity;
using PackageViewer.Models;

namespace PackageViewer.Views;

public enum MessageDialogIcon {
  Information,
  Error,
}

/// <summary>
/// A simple message dialog (heading, text and optional collapsible details)
/// </summary>
public partial class MessageDialog : Window {
  public MessageDialog() {
    this.InitializeComponent();
    this.Title = RuntimeHelpers.ApplicationName;
  }

  public MessageDialog(string heading, string text, MessageDialogIcon icon, string? details) : this() {
    this.headingText.Text = heading;
    this.messageText.Text = text;
    this.iconImage.Source = icon == MessageDialogIcon.Error ? Icons.Error : Icons.Info;

    if (!string.IsNullOrEmpty(details)) {
      this.detailsText.Text = details;
      this.detailsExpander.IsVisible = true;
      this.detailsExpander.Expanding += (_, _) => this.detailsExpander.Header = "Hide details";
      this.detailsExpander.Collapsing += (_, _) => this.detailsExpander.Header = "Show details";
    }
  }

  /// <summary>
  /// Shows the dialog modally and completes when it is closed
  /// </summary>
  public static Task ShowAsync(Window owner, string heading, string text, MessageDialogIcon icon, string? details = null) {
    var dialog = new MessageDialog(heading, text, icon, details);
    return dialog.ShowDialog(owner);
  }

  private void OkButton_Click(object? sender, RoutedEventArgs e) => this.Close();
}
