using System;
using System.Globalization;

namespace FolderIconChangerWPF.ValueConverters
{
    public class LexEnumConverter : BaseValueConverter
    {
        public bool PrependType { get; set; } = true;
        public string Separator { get; set; } = "_";
        public string Prefix { get; set; }
        public string ValueFormat { get; set; }

        string GetKey(object value)
        {
            if (!(value is Enum enumValue)) return null;

            string key = enumValue.ToString();
            if (PrependType)
            {
                key = enumValue.GetType().Name + Separator + key;
            }
            if (!string.IsNullOrEmpty(Prefix))
            {
                key = Prefix + Separator + key;
            }
            return key;
        }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = GetKey(value);
            if (string.IsNullOrEmpty(key)) return value?.ToString();
            string format = (parameter as string) ?? this.ValueFormat;
            if (string.IsNullOrEmpty(format))
            {
                return LocalizationProvider.GetLocalizedString(key);
            }
            else
            {
                return StringFormatConverter.FormatMethod(format, culture, LocalizationProvider.GetLocalizedString(key));
            }
            //var lString = LocalizationProvider.GetLocalizedString(key);
        }
    }
}
