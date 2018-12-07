using System;
using System.Globalization;
using System.Diagnostics;

namespace FolderIconChangerWPF.ValueConverters
{
    public class BitmapSDToSWConverter : BaseValueConverter<BitmapSDToSWConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();

            if (!(value is System.Drawing.Bitmap valImage)) return null;
            return valImage?.ToSWBitmapImage();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
