using System;
using System.Globalization;
using System.Windows.Data;

namespace FolderIconChangerWPF.ValueConverters
{
    public class DateTimeUTCToLocalStringConverter : BaseValueConverter<DateTimeUTCToLocalStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = value as DateTime?;
            if (!dateTime.HasValue) return null;

            var local = dateTime.Value.ToLocalTime();
            var format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                return local.ToString(format);
            }
            return local.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var localFormated = value as string;
            if (!string.IsNullOrEmpty(localFormated))
            {
                //return ((DateTime)value).ToUniversalTime();
                var format = parameter as string;
                var resDate = default(DateTime);
                if (!string.IsNullOrEmpty(format))
                {
                    if (DateTime.TryParseExact(localFormated, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out resDate))
                    {
                        return resDate.ToUniversalTime();
                    }
                    if (targetType == typeof(DateTime))
                    {
                        return default(DateTime);
                    }
                    return null;
                }
                if (DateTime.TryParse(localFormated, out resDate))
                {
                    return resDate.ToUniversalTime();
                }
                if (targetType == typeof(DateTime))
                {
                    return default(DateTime);
                }
                return null;
            }
            if (targetType == typeof(DateTime))
            {
                return default(DateTime);
            }
            return null;
        }
    }
}
