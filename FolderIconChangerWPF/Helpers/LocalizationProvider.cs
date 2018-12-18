using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using WPFLocalizeExtension.Extensions;

namespace FolderIconChangerWPF
{
    public static class LocalizationProvider
    {

        /// <summary>
        /// Gets Localized String by key. (Formated)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetLocalizedString(string key, params string[] args)
        {
            var LStr = GetLocalizedValue<string>(key);
            if (args == null || args.Length == 0) return LStr;
            return string.Format(LStr, args);
        }
        public static string GetLocalizedString(string key, Func<string> defaultValue, params string[] args)
        {
            var LStr = GetLocalizedString(key, defaultValue);
            if (args == null || args.Length == 0) return LStr;
            return string.Format(LStr, args);
        }
        /// <summary>
        /// Gets Localized String by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLocalizedString(string key) => GetLocalizedValue<string>(key);
        public static string GetLocalizedString(string key, Func<string> defaultValue)
        {
            var res = GetLocalizedString(key);
            return string.IsNullOrEmpty(res) ? defaultValue() : res;
        }

        public static T GetLocalizedValue<T>(string key) => LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":Resources:" + key);

        public static BitmapImage GetLangFlagImage() => LocExtension.GetLocalizedValue<BitmapImage>("LangFlagBase64", new ValueConverters.Base64ImageConverter());
        public static BitmapImage GetLangFlagImage(this CultureInfo culture) => LocExtension.GetLocalizedValue<BitmapImage>("LangFlagBase64", culture, new ValueConverters.Base64ImageConverter());

        public static FlowDirection GetFlowDirection() => LocExtension.GetLocalizedValue<FlowDirection>("FlowDirection");
        public static FlowDirection GetFlowDirection(this CultureInfo culture) => LocExtension.GetLocalizedValue<FlowDirection>("FlowDirection", culture);
    }
}
