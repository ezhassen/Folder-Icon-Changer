using System;
using System.Windows.Media.Imaging;

namespace FolderIconChangerWPF.ValueConverters
{
    public class Base64ImageConverter: BaseValueConverter<Base64ImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return MediaHelper.FromBase64(value as string);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value as BitmapImage)?.ToBase64();
        }
    }
}
