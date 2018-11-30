using System.Collections.ObjectModel;
using System.Globalization;

namespace FolderIconChangerWPF.Services
{
    public interface ISettingsService
    {
        CultureInfo SelectedCulture { get; set; }
        ThemeInfo SelectedTheme { get; set; }
        ObservableCollection<CultureInfo> SupportedCultures { get; }
        ObservableCollection<ThemeInfo> Themes { get; }

        void RefreshCurrentCulture();
        void RefreshCurrentTheme();
    }
}