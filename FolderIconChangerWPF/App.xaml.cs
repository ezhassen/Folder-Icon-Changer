using System.Windows;

namespace FolderIconChangerWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //
            Services.SettingsService.Instance.RefreshAllSettings();
            //
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //FolderIconChangerWPF.Properties.Settings.Default.Save();
            Services.SettingsService.Instance.Save();
            //
            base.OnExit(e);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
#if !DEBUG
            e.Handled = true;
            if (e.Exception is TaskCanceledException) return;
            MessageBox.Show(e.Exception.ToString());
#endif
        }
    }
}
