using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using WPFLocalizeExtension.Engine;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FolderIconChangerWPF.ViewModels;

namespace FolderIconChangerWPF.Services
{
    public class SettingsService : INotifyPropertyChanged, ISettingsService
    {
        public static SettingsService Instance { get; } = new SettingsService();


        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                OnPropertyChanged(propertyName);
            }
        }

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        public void RefreshAllSettings()
        {
            this.RefreshCurrentCulture();
            this.RefreshCurrentTheme();
            this.RefreshOtherSettings();
        }
        public void RefreshOtherSettings()
        {
            this.IsTopMost = IsTopMost;
        }
        //public SettingsService()
        //{
        //    LocalizeDictionary.Instance.IncludeInvariantCulture = false;
        //}
        //
        CultureInfo _selectedCulture;
        public CultureInfo SelectedCulture
        {
            get
            {
                if (_selectedCulture == null)
                {
                    var savedCul = Properties.Settings.Default.SelectedCulture;
                    if (string.IsNullOrEmpty(savedCul)) savedCul = "en";
                    //_selectedCulture = new CultureInfo(savedCul);
                    var findInSupp = SupportedCultures.FirstOrDefault(cul => cul.Name.Equals(savedCul, StringComparison.OrdinalIgnoreCase));
                    _selectedCulture = findInSupp ?? new CultureInfo(savedCul);
                }
                return _selectedCulture;
            }

            set
            {
                if (_selectedCulture == value) return;
                _selectedCulture = value;
                if (_selectedCulture == null) _selectedCulture = new CultureInfo("en");
                Properties.Settings.Default.SelectedCulture = _selectedCulture.Name;
                //RefreshCurrentCulture();
                LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                LocalizeDictionary.Instance.Culture = _selectedCulture;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CultureInfo> SupportedCultures
        {
            get
            {
                if (LocalizeDictionary.Instance.IncludeInvariantCulture) LocalizeDictionary.Instance.IncludeInvariantCulture = false;
                return LocalizeDictionary.Instance.MergedAvailableCultures;
            }
        }

        public void RefreshCurrentCulture()
        {
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = SelectedCulture ?? new CultureInfo("en");
        }




        DelegateCommand _ChangeSelectedCultureCommand;
        public DelegateCommand ChangeSelectedCultureCommand
            => _ChangeSelectedCultureCommand ?? (_ChangeSelectedCultureCommand = new DelegateCommand((cul) =>
            {
                if (!(cul is CultureInfo selectedCul)) return;
                this.SelectedCulture = selectedCul;
            }));


        ThemeInfo _SelectedTheme;
        public ThemeInfo SelectedTheme
        {
            get
            {
                if (_SelectedTheme == null)
                {
                    var savedT = Properties.Settings.Default.SelectedThemeName;
                    if (string.IsNullOrEmpty(savedT)) savedT = ThemeInfo.Default.Name;
                    var find = Themes.FirstOrDefault(th => th.Name.Equals(savedT, StringComparison.OrdinalIgnoreCase));
                    _SelectedTheme = find ?? ThemeInfo.Default;
                }

                return _SelectedTheme;
            }
            set
            {
                if (_SelectedTheme == value) return;
                _SelectedTheme = value;
                Properties.Settings.Default.SelectedThemeName = value.Name;
                value.ApplyTheme();
                OnPropertyChanged();
                Properties.Settings.Default.Save();
            }
        }
        DelegateCommand _ChangeSelectedThemeCommand;
        public DelegateCommand ChangeSelectedThemeCommand
            => _ChangeSelectedThemeCommand ?? (_ChangeSelectedThemeCommand = new DelegateCommand((cul) =>
            {
                if (!(cul is ThemeInfo selectedTh)) return;
                this.SelectedTheme = selectedTh;
            }));

        public void RefreshCurrentTheme()
        {
            SelectedTheme.ApplyTheme();
            OnPropertyChanged(nameof(SelectedTheme));
        }


        public ObservableCollection<ThemeInfo> Themes => ThemeHelper.Themes;

        public bool IsTopMost
        {
            get { return Properties.Settings.Default.TopMost; }
            set
            {
                if (Properties.Settings.Default.TopMost != value)
                {
                    Properties.Settings.Default.TopMost = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
                //
                var mainW = Application.Current.MainWindow;
                if (mainW != null)
                {
                    mainW.Topmost = value;
                }
            }
        }


        public bool CopyIconToFolder
        {
            get { return Properties.Settings.Default.CopyIconToFolder; }
            set
            {
                if (Properties.Settings.Default.CopyIconToFolder != value)
                {
                    Properties.Settings.Default.CopyIconToFolder = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        public bool HideIcon
        {
            get { return Properties.Settings.Default.HideIcon; }
            set
            {
                if (Properties.Settings.Default.HideIcon != value)
                {
                    Properties.Settings.Default.HideIcon = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }
        public bool ShowCurrentFolderForIconFromImage
        {
            get { return Properties.Settings.Default.ShowCurrentFolderForIconFromImage; }
            set
            {
                if (Properties.Settings.Default.ShowCurrentFolderForIconFromImage != value)
                {
                    Properties.Settings.Default.ShowCurrentFolderForIconFromImage = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }
    }
}
