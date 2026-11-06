using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;

namespace PackageViewer.Models;

/// <summary>
/// A folder shown in the folder tree
/// </summary>
public sealed class FolderNode : INotifyPropertyChanged {
  private bool _isExpanded;

  public FolderNode(string name, string path, FolderNode? parent) {
    this.Name = name;
    this.Path = path;
    this.Parent = parent;
  }

  /// <summary>
  /// Display name (folder name, or the Title ID for the root)
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Folder path inside the package, '/' separated with no leading or trailing slash ("" for the root)
  /// </summary>
  public string Path { get; }

  public FolderNode? Parent { get; }

  public ObservableCollection<FolderNode> Children { get; } = [];

  public bool IsExpanded {
    get => this._isExpanded;
    set {
      if (this._isExpanded == value) return;
      this._isExpanded = value;
      this.OnPropertyChanged();
      this.OnPropertyChanged(nameof(this.Icon));
    }
  }

  public Bitmap Icon => this.IsExpanded ? Icons.FolderOpen : Icons.FolderClose;

  public event PropertyChangedEventHandler? PropertyChanged;

  private void OnPropertyChanged([CallerMemberName] string? name = null) {
    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
  }
}
