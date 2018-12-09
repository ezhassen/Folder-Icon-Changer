using System.Windows;

namespace FolderIconChangerWPF.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : BasePage
    {
        public MainPage()
        {
            InitializeComponent();
            ViewModels.MainPageViewModel.Instance.OwnerWindow = Application.Current.MainWindow;
        }
    }
}
