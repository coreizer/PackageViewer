using Avalonia;

namespace PackageViewer;

internal static class Program {
  /// <summary>
  ///  The main entry point for the application.
  /// </summary>
  [STAThread]
  static void Main(string[] args) {
    BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
  }

  /// <summary>
  /// Avalonia configuration (also used by the previewer, do not remove)
  /// </summary>
  public static AppBuilder BuildAvaloniaApp() =>
    AppBuilder.Configure<App>()
      .UsePlatformDetect()
      .LogToTrace();
}
