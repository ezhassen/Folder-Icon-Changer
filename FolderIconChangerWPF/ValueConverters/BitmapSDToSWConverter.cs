using System;
using System.Globalization;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FolderIconChangerWPF.ValueConverters
{
    public class BitmapSDToSWAsyncConverter : BaseValueConverter<BitmapSDToSWAsyncConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();

            if (!(value is System.Drawing.Bitmap valImage)) return null;
            return Task.Run(() => valImage?.ToSWBitmapImage());
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
