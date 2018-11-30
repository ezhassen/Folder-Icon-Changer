using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FolderIconChangerWPF.Interfaces
{
    public interface ITreeItem<T> : INotifyPropertyChanged where T : class
    {
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        T Parent { get; }
        ObservableCollection<T> Children { get; }
    }
}
