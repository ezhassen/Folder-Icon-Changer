using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            //Services.SettingsService.Instance.RefreshCurrentCulture();
            //Services.SettingsService.Instance.RefreshCurrentTheme();
            //Services.SettingsService.Instance.RefreshOtherSettings();
            //
        }

        protected override void OnExit(ExitEventArgs e)
        {
            FolderIconChangerWPF.Properties.Settings.Default.Save();
            //
            base.OnExit(e);
        }
    }
}
