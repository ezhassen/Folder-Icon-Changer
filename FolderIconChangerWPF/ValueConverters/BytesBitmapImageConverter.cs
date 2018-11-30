using System;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace FolderIconChangerWPF.ValueConverters
{
    //public class BytesWriteableBitmapConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value as byte[])?.byteArrayToBitmapImage();

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (value as WriteableBitmap)?.AsByteArray();
    //}
    public class BytesBitmapImageConverter : BaseValueConverter<BytesBitmapImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value as byte[])?.ToBitmapImage();

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (value as BitmapImage)?.ToByteArray();
    }
}
