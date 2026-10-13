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
  /// Shows an error message with TaskDialog (exception details are shown in the expander)
  /// </summary>
  /// <param name="owner">Owner window (shown without an owner if null)</param>
  /// <param name="heading">Heading text</param>
  /// <param name="ex">Exception to display</param>
  public static void ShowError(IWin32Window? owner, string heading, Exception ex) {
    var page = new TaskDialogPage() {
      Caption = ApplicationName,
      Heading = heading,
      Text = ex.Message,
      Icon = TaskDialogIcon.Error,
      Buttons = { TaskDialogButton.OK },
      Expander = new TaskDialogExpander() {
        Text = ex.ToString(),
        CollapsedButtonText = "詳細を表示",
        ExpandedButtonText = "詳細を隠す",
      },
    };

    if (owner != null) {
      TaskDialog.ShowDialog(owner, page);
    } else {
      TaskDialog.ShowDialog(page);
    }
  }
}
