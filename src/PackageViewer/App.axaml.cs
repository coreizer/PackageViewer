using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PackageViewer.Views;

namespace PackageViewer;

public partial class App : Application {
  public override void Initialize() {
    AvaloniaXamlLoader.Load(this);
  }

  public override void OnFrameworkInitializationCompleted() {
    if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
      var window = new MainWindow();
      desktop.MainWindow = window;

      // Open the package passed on the command line (e.g. a file dropped onto the executable)
      var path = desktop.Args?.FirstOrDefault();
      if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
        window.Opened += async (_, _) => await window.OpenPackageAsync(path);
      }
    }

    base.OnFrameworkInitializationCompleted();
  }
}
