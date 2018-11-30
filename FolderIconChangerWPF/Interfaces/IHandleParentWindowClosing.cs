using System;
using System.Windows;

namespace FolderIconChangerWPF
{
    public interface IHandleParentWindowClosing
    {
        //BaseViewModel AttachedViewModel { get; }

        void OnClosingCommandChanged(DependencyPropertyChangedEventArgs e);
        void OnClosedCommandChanged(DependencyPropertyChangedEventArgs e);
        void ParentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e);
        void ParentWindow_Closed(object sender, EventArgs e);
    }
}
