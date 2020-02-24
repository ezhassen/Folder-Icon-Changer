using FolderIconChangerWPF.ViewModels;
using MahApps.Metro.Controls;

namespace FolderIconChangerWPF.Windows
{
    /// <summary>
    /// Interaction logic for SelectIconWindow.xaml
    /// </summary>
    public partial class SelectIconWindow : MetroWindow
    {
        public SelectIconWindow()
        {
            InitializeComponent();
            //
            ViewModel.OnClosingCommand = new DelegateCommand(_ => ViewModel.Dispose());
            ViewModel.CloseCommand = new DelegateCommand(() => this.Close());
            ViewModel.OwnerWindow = this;
        }
    }
}
