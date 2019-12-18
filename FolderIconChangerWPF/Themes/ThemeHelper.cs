using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using FolderIconChangerWPF.ViewModels;
using MahApps.Metro;

namespace FolderIconChangerWPF
{
    public class ThemeInfo : IEqualityComparer<ThemeInfo>, IEquatable<ThemeInfo>
    {
        public const string DefaultName = "AppLight";
        public string Name { get; }
        public string LocalizedDisplayName => LocalizationProvider.GetLocalizedString(Name, ()=> DisplayName);
        public string DisplayName { get; }
        public string RelativePath { get; }
        public string FullPath => $"pack://application:,,,/{nameof(FolderIconChangerWPF)};{RelativePath}";

        public string MetroThemeName { get; set; }

        public Color Foreground { get; }
        public Brush ForegroundBrush => new SolidColorBrush(Foreground);
        public Color Background { get; }
        public Brush BackgroundBrush => new SolidColorBrush(Background);

        public ThemeInfo(string name, string displayName, string relativePath, string foreground, string background, string metroThemeName)
        {
            Background = FolderIconChangerWPF.MediaHelper.SWMColorFromHEX(background);
            Foreground = MediaHelper.SWMColorFromHEX(foreground);
            RelativePath = relativePath;
            DisplayName = displayName;
            Name = name;
            MetroThemeName = metroThemeName;
        }
        public ThemeInfo(string name, string displayName, string relativePath, Color foreground, Color background)
        {
            Background = background;
            Foreground = foreground;
            RelativePath = relativePath;
            DisplayName = displayName;
            Name = name;
        }


        DelegateCommand _ApplyThemeCommand;
        public DelegateCommand ApplyThemeCommand
            => _ApplyThemeCommand ?? (_ApplyThemeCommand = new DelegateCommand(ApplyTheme));
        public void ApplyTheme()
        {
            ThemeHelper.ApplyTheme(Name, RelativePath, this.MetroThemeName);
            //ThemeHelper.ApplyTheme(Name, FullPath);
        }

        public bool Equals(ThemeInfo x, ThemeInfo y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(ThemeInfo obj) => obj.GetHashCode();
        public bool Equals(ThemeInfo other) => (other is null) ? false : this.Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

        static ThemeInfo defaultTheme;
        public static ThemeInfo Default {
            get {
                if (defaultTheme == null) defaultTheme = new ThemeInfo("AppLight", "App Light", "Themes/AppLight.xaml", "#FF000000", "#FFFFFFFF", "BaseLight");
                return defaultTheme;
            }
        }

    }
    public static class ThemeHelper
    {
        /*
         
             Uri uriRD = new Uri("/Themes/Luna.Homestead.xaml", System.UriKind.Relative);
Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(uriRD));

OTOH, if the RD is located in a different assembly, use the pack-syntax:

Uri uriRD = new Uri("YourAssemblyName;component/Themes/Luna.Homestead.xaml", System.UriKind.Relative);
Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(uriRD));


             
             
             */
        //public static ThemeNames CurrentTheme
        //{
        //    get
        //    {
        //        //Application.Current.Resources.MergedDictionaries.FirstOrDefault(rd => rd.Source);
        //    }

        //    set
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //}
        static ObservableCollection<ThemeInfo> themes;
        public static ObservableCollection<ThemeInfo> Themes {
            get {
                if (themes == null)
                {
                    themes = new ObservableCollection<ThemeInfo>();
                    themes.Add(ThemeInfo.Default);
                    themes.Add(new ThemeInfo("AppDark", "App Dark", "Themes/AppDark.xaml", "#FFFFFFFF", "#FF2D2D30", "BaseDark"));
                    themes.Add(new ThemeInfo("AppDarkBlue", "App Dark Blue", "Themes/AppDarkBlue.xaml", "#FFFFFFFF", "#FF025A9D", "BaseDark"));
                }
                return themes;
            }
        }

        public static void ApplyThemeByName(string themeName)
        {
            Themes.FirstOrDefault(t => t.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase))?.ApplyTheme();
        }
        public static void ApplyTheme(string name, string path, string metroThemeName = "BaseLight")
        {
            //var uriRD = new Uri(rePath, System.UriKind.Relative);
            ////var findOldTheme = Application.Current.Resources.MergedDictionaries.FirstOrDefault(rd => rd.Source.OriginalString.Contains("Themes"));
            ////if (findOldTheme != null) Application.Current.Resources.MergedDictionaries.Remove(findOldTheme);
            //Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(uriRD));
            //Application.Current.MainWindow?.InvalidateVisual();
            ////Application.Current.MainWindow?.RefreshUI();
            /////
            ///
            //var sss = ThemeManager.DetectTheme(Application.Current);

            var theme = ThemeManager.GetTheme(name);
            if (theme is null)
            {
                ThemeManager.AddTheme(new Uri(path, UriKind.RelativeOrAbsolute));
                //ThemeManager.AddAppTheme(name, new Uri(path, UriKind.RelativeOrAbsolute));
                //ThemeManager.ChangeAppTheme(Application.Current, name);
                theme = ThemeManager.GetTheme(name);
            }
            //var MetroTheme = ThemeManager.GetAppTheme(metroThemeName);

            //if (MetroTheme != null) ThemeManager.ChangeAppStyle(Application.Current, sss.Item2, MetroTheme);
            if (theme is null) return;
            ThemeManager.ChangeTheme(Application.Current, theme);
            //ThemeManager.ChangeAppStyle(Application.Current, sss.Item2, theme);
            //foreach (Window window in Application.Current.Windows)
            //{
            //    ThemeManager.ChangeAppStyle(window, sss.Item2, theme);
            //    window.InvalidateVisual();
            //    var mw = window as MahApps.Metro.Controls.MetroWindow;
            //    if (mw != null)
            //    {
            //        mw.TitleTemplate.Dispatcher?.Invoke(DispatcherPriority.Render, EmptyDelegate);
            //        mw.Dispatcher?.Invoke(DispatcherPriority.Render, EmptyDelegate);
            //    }
            //}
            //if (Application.Current.MainWindow != null) ThemeManager.ChangeAppStyle(Application.Current.MainWindow, sss.Item2, theme);
        }

        private static Action EmptyDelegate = delegate () { };


        //public static void RefreshUI(this UIElement uiElement)
        //{
        //    uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        //}


        //public static void AddAccentsToThemeManager()
        //{
        //    //https://mahapps.com/guides/styles.html
        //    var _themes = Themes;

        //    foreach (var th in _themes)
        //    {
        //        ThemeManager.AddAccent(th.Name, new Uri(th.FullPath));
        //    }
        //    // get the current app style (theme and accent) from the application
        //    Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);
        //    //theme.Item1.
        //    // now change app style to the custom accent and current theme
        //    ThemeManager.ChangeAppStyle(Application.Current,
        //                                ThemeManager.GetAccent("CustomAccent1"),
        //                                theme.Item1);

        //}
    }
}
