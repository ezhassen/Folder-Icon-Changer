using System;
using System.Linq;
using System.Globalization;
using WPFLocalizeExtension.Engine;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using FolderIconChangerWPF.ViewModels;
using System.Collections.Specialized;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Input;

namespace FolderIconChangerWPF.Services
{
    public class SettingsService : ApplicationSettingsBase, ISettingsService
    {
        //public static SettingsService Instance { get; } = new SettingsService();

        static SettingsService _Instance;

        public static SettingsService Instance
        {
            get
            {
                if (_Instance is null) _Instance = (SettingsService)ApplicationSettingsBase.Synchronized(new SettingsService());
                return _Instance;
            }
            set { _Instance = value; }
        }

        #region NotifyPropertyChanged


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
            base.OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
            //this.IsTopMost = IsTopMost;
            AllowClearRecentFiles = this.RecentFiles?.Count > 0;
            AllowClearRecentFolders = this.RecentFolders?.Count > 0;
        }
        //public SettingsService()
        //{
        //    LocalizeDictionary.Instance.IncludeInvariantCulture = false;
        //}
        //
        CultureInfo _selectedCulture;

        [UserScopedSetting, SettingsSerializeAs(SettingsSerializeAs.String), DefaultSettingValue("en")]
        public string SelectedCultureName
        {
            get
            {
                return this["SelectedCultureName"] as string;
            }
            set
            {
                this["SelectedCultureName"] = value;
            }
        }

        public CultureInfo SelectedCulture
        {
            get
            {
                if (_selectedCulture == null)
                {
                    var savedCul = SelectedCultureName;
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
                SelectedCultureName = _selectedCulture.Name;
                //RefreshCurrentCulture();
                LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                LocalizeDictionary.Instance.Culture = _selectedCulture;
                this.Save();
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


        [UserScopedSetting, SettingsSerializeAs(SettingsSerializeAs.String), DefaultSettingValue("AppDarkBlue")]
        public string SelectedThemeName
        {
            get
            {
                return this["SelectedThemeName"] as string;
            }
            set
            {
                this["SelectedThemeName"] = value;
            }
        }

        ThemeInfo _SelectedTheme;
        public ThemeInfo SelectedTheme
        {
            get
            {
                if (_SelectedTheme == null)
                {
                    var savedT = SelectedThemeName;
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
                SelectedThemeName = value.Name;
                value.ApplyTheme();
                OnPropertyChanged();
                this.Save();
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

        [UserScopedSetting, DefaultSettingValue("False")]
        public bool IsTopMost
        {
            get
            {
                //return (bool)this["IsTopMost"];
                var val = (bool)this["IsTopMost"];
                var mainW = Application.Current.MainWindow;
                if (mainW != null && val != mainW.Topmost) mainW.Topmost = val;
                return val;
            }
            set
            {
                if (IsTopMost != value)
                {
                    this["IsTopMost"] = value;
                    //this.Save();
                    OnPropertyChanged(); //uses CallerMemberName
                }
                //
                var mainW = Application.Current.MainWindow;
                if (mainW != null && value != mainW.Topmost) mainW.Topmost = value;
            }
        }


        [UserScopedSetting, DefaultSettingValue("True")]
        public bool CopyIconToFolder
        {
            get { return ((bool)(this["CopyIconToFolder"])); }
            set
            {
                if (CopyIconToFolder != value)
                {
                    this["CopyIconToFolder"] = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        [UserScopedSetting, DefaultSettingValue("True")]
        public bool HideIcon
        {
            get { return ((bool)(this["HideIcon"])); }
            set
            {
                if (HideIcon != value)
                {
                    this["HideIcon"] = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }
        [UserScopedSetting, DefaultSettingValue("True")]
        public bool ShowCurrentFolderForIconFromImage
        {
            get { return ((bool)(this["ShowCurrentFolderForIconFromImage"])); }
            set
            {
                if (ShowCurrentFolderForIconFromImage != value)
                {
                    this["ShowCurrentFolderForIconFromImage"] = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }



        [UserScopedSetting, SettingsSerializeAs(SettingsSerializeAs.Binary)]
        public ObservableCollection<string> RecentFolders
        {
            get
            {
                if (this["RecentFolders"] is null)
                {
                    this["RecentFolders"] = new ObservableCollection<string>();
                    this.Save();
                }
                return this["RecentFolders"] as ObservableCollection<string>;
            }
            set
            {
                //if (_RecentFolders != value)
                //{
                this["RecentFolders"] = value;
                this.Save();
                OnPropertyChanged(); //uses CallerMemberName
                //}
                AllowClearRecentFolders = value?.Count > 0;
            }
        }

        private string[] _DefaultRecentFiles;

        public string[] DefaultRecentFiles
        {
            get
            {
                if (_DefaultRecentFiles is null) _DefaultRecentFiles = new string[] { Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shell32.dll") };
                return _DefaultRecentFiles;
            }
            protected set { _DefaultRecentFiles = value; }
        }

        public ICollection<string> RecentFilesWithDefaults
        {
            get
            {
                if (this.RecentFiles is null || this.RecentFiles.Count == 0) return this.DefaultRecentFiles;
                //var res = this.DefaultRecentFiles;
                //var count = RecentFiles.Count == 0 ? 0 : (RecentFiles.Count - 1);
                //var arr = new string[count];
                //RecentFiles.CopyTo(res, 0);
                var rece = RecentFiles.ToList();

                rece.AddRange(this.DefaultRecentFiles.Where(r => !rece.Contains(r, new StringIEqualityComparer(StringComparison.OrdinalIgnoreCase))));
                return rece;
                //return res;
            }
        }

        [UserScopedSetting, SettingsSerializeAs(SettingsSerializeAs.Binary)]
        public ObservableCollection<string> RecentFiles
        {
            get
            {
                if (this["RecentFiles"] is null)
                {
                    this["RecentFiles"] = new ObservableCollection<string>();
                    this.Save();
                }
                return this["RecentFiles"] as ObservableCollection<string>;
            }
            set
            {
                //if (_RecentFolders != value)
                //{
                this["RecentFiles"] = value;
                this.Save();
                OnPropertyChanged(); //uses CallerMemberName
                OnPropertyChanged(nameof(RecentFilesWithDefaults));
                //}
                AllowClearRecentFiles = value?.Count > 0;
            }
        }

        public void AddRecentFolder(string path) => this.RecentFolders = AddRecent(RecentFolders, path);
        public void AddRecentFile(string path) => this.RecentFiles = AddRecent(RecentFiles, path);

        ObservableCollection<string> AddRecent(ObservableCollection<string> stringCollection, string recent, int max = 20)
        {
            var res = stringCollection;
            if (res is null)
            {
                res = new ObservableCollection<string>
                {
                    recent
                };
                return res;
            }
            //DefaultRecentFiles.Contains("", new StringIEqualityComparer(StringComparison.OrdinalIgnoreCase));
            var stringIndex = StringIndex(res, recent);
            if (stringIndex >= 0)
            {
                //To Move to top of the list
                //res.Insert(0, recent);
                //res.RemoveAt(stringIndex + 1);
                //TODO: Fix TargetFolder Empty Issue When Moving to top of the recent list

                if (stringIndex > 0)
                {
                    var newRes = new ObservableCollection<string>(res);
                    newRes.Move(stringIndex, 0);
                    return newRes;
                }
            }
            else
            {
                if (res.Count != 0 && res.Count >= max)
                {
                    //Remove Old recent
                    res.RemoveAt(res.Count - 1);
                }
                //Add New recent
                res.Insert(0, recent);
            }
            return res;
        }

        int StringIndex(ObservableCollection<string> stringCollection, string str, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            for (int i = 0; i < stringCollection.Count; i++)
            {
                var item = stringCollection[i];
                if (item is null)
                {
                    if (str is null) return i;
                    continue;
                }
                if (item.Equals(str, stringComparison)) return i;
            }
            //foreach (var item in stringCollection)
            //{
            //    if (item is null)
            //    {
            //        if (str is null) return true;
            //        continue;
            //    }
            //    if (item.Equals(str, stringComparison)) return true;
            //}
            return -1;
        }


        DelegateCommand _ClearRecentsFoldersCommand;
        public DelegateCommand ClearRecentFoldersCommand
            => _ClearRecentsFoldersCommand ?? (_ClearRecentsFoldersCommand = new DelegateCommand(() =>
            {
                if (!(Mouse.OverrideCursor is null)) return;
                Mouse.OverrideCursor = Cursors.Wait;
                this.RecentFolders?.Clear();
                AllowClearRecentFolders = false;
                Save();
                Mouse.OverrideCursor = null;
            }));

        bool _AllowClearRecentFolders;
        public bool AllowClearRecentFolders
        {
            get { return _AllowClearRecentFolders; }
            set
            {
                if (_AllowClearRecentFolders != value)
                {
                    _AllowClearRecentFolders = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        DelegateCommand _ClearRecentsFilesCommand;
        public DelegateCommand ClearRecentFilesCommand
            => _ClearRecentsFilesCommand ?? (_ClearRecentsFilesCommand = new DelegateCommand(() =>
            {
                if (!(Mouse.OverrideCursor is null)) return;
                Mouse.OverrideCursor = Cursors.Wait;
                this.RecentFiles?.Clear();
                AllowClearRecentFiles = false;
                Save();
                Mouse.OverrideCursor = null;
            }));

        bool _AllowClearRecentFiles;
        public bool AllowClearRecentFiles
        {
            get { return _AllowClearRecentFiles; }
            set
            {
                if (_AllowClearRecentFiles != value)
                {
                    _AllowClearRecentFiles = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        public override void Save()
        {
            FolderIconChangerWPF.Properties.Settings.Default.Save();
            base.Save();
        }
    }
}
