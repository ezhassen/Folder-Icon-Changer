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
            if (e.Exception is System.Threading.Tasks.TaskCanceledException) return;
            if (e.Exception is System.OperationCanceledException) return;
            Helpers.ModernMessageBox.Show(e.Exception.ToString(), GetLocalizedStringSafe("Error") ?? "Error", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
        }

        static string GetLocalizedStringSafe(string key)
        {
            try { return LocalizationProvider.GetLocalizedString(key); } catch { return null; }
        }
    }
}
