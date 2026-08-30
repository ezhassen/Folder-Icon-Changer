using FolderIconChangerWPF.Services;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FolderIconChangerWPF.ViewModels
{
    public class AboutPageViewModel : BaseViewModel
    {
        public AboutPageViewModel()
        {
            SettingsService.Instance.PropertyChanged += Instance_PropertyChanged;
        }

        private void Instance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SettingsService.SelectedTheme))
            {
                OnPropertyChanged(nameof(GitHubIcon));
            }
        }

        public string AppName { get; set; }
        public string DevName
        {
            get
            {
                var LocName = LocalizationProvider.GetLocalizedString("DevName", defaultValue: () => "Ezz Hassan");
                //var res = $"{LocName} (ezhassen)";

                return $"{LocName} (ezhassen)";
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }


        DelegateCommand _OpenLinkCommand;
        public DelegateCommand OpenLinkCommand
            => _OpenLinkCommand ?? (_OpenLinkCommand = new DelegateCommand((param) =>
            {
                if (!(param is string link) || string.IsNullOrEmpty(link)) return;

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo(link)
                    {
                        UseShellExecute = true
                    }

                };
                //process.
                process.Start();
            }));

        public Uri GitHubIcon => new Uri($"pack://application:,,,/Resources/github_{(SettingsService.Instance.SelectedThemeIsDark ? "dark" : "light")}.png");
    }
}
