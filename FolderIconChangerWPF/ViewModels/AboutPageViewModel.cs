using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FolderIconChangerWPF.ViewModels
{
    public class AboutPageViewModel : BaseViewModel
    {
        public string AppName { get; set; }
        public string DevName
        {
            get
            {
                var LocName = LocalizationProvider.GetLocalizedString("DevName", defaultValue: () => "Ezz Hasan");
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
    }
}
