using Ezz_Helper.Drawing.IconsManager;
using FolderIconChangerWPF.ViewModels;
using MahApps.Metro.Controls;

namespace FolderIconChangerWPF.Windows
{
    /// <summary>
    /// Interaction logic for IconInfoImagesWindow.xaml
    /// </summary>
    public partial class IconInfoImagesWindow : MetroWindow
    {
        public IconInfoImagesWindow()
        {
            InitializeComponent();
        }
        public IconInfoImagesWindow(IconInfoImagesViewModel _viewModel) : this()
        {
            ViewModel = _viewModel;
        }
        public IconInfoImagesWindow(IconInfo iconInfo) : this(new IconInfoImagesViewModel(iconInfo))
        {
        }

        private IconInfoImagesViewModel viewModel;

        public IconInfoImagesViewModel ViewModel
        {
            get { return viewModel; }
            set
            {
                viewModel = value;
                this.DataContext = this.viewModel;
            }
        }

    }
}
