using Avalonia.Controls;
using PackageViewer.Views;

namespace PackageViewer;

/// <summary>
/// Application-wide helpers
/// </summary>
public static class RuntimeHelpers {
  /// <summary>
  /// Application name shown in window titles and dialogs
  /// </summary>
  public static string ApplicationName {
    get => "PS3 Package Viewer";
  }

  /// <summary>
  /// Application version shown in the title bar and the About dialog
  /// </summary>
  public static string Version {
    get => "1.0.0";
  }

  /// <summary>
  /// Shows an error message (exception details are shown in the expander)
  /// </summary>
  /// <param name="owner">Owner window</param>
  /// <param name="heading">Heading text</param>
  /// <param name="ex">Exception to display</param>
  public static Task ShowErrorAsync(Window owner, string heading, Exception ex) {
    return MessageDialog.ShowAsync(owner, heading, ex.Message, MessageDialogIcon.Error, ex.ToString());
  }

  /// <summary>
  /// Shows an information message
  /// </summary>
  /// <param name="owner">Owner window</param>
  /// <param name="heading">Heading text</param>
  /// <param name="text">Message text</param>
  public static Task ShowInformationAsync(Window owner, string heading, string text) {
    return MessageDialog.ShowAsync(owner, heading, text, MessageDialogIcon.Information);
  }
}
