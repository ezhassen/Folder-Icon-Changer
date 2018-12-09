using FolderIconChangerWPF.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            ViewModel.CloseCommand = new DelegateCommand(() => this.Close());
            ViewModel.OwnerWindow = this;
        }
    }
}
