using System;
using System.Globalization;


namespace FolderIconChangerWPF.ValueConverters
{
    public class DateTimeUTCToLocalConverter : BaseValueConverter<DateTimeUTCToLocalConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is DateTime) return ((DateTime)value).ToLocalTime();
            //var dateT = value as DateTime?;
            //if (!dateT.HasValue) return dateT.Value.ToLocalTime();
            //return null;
            return (value as DateTime?)?.ToLocalTime();
        }


        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is DateTime) return ((DateTime)value).ToUniversalTime();
            //var dateT = value as DateTime?;
            //if (!dateT.HasValue) return dateT.Value.ToUniversalTime();
            //return null;
            return (value as DateTime?)?.ToUniversalTime();
        }

    }
}
