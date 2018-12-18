using System;
using System.Globalization;

namespace FolderIconChangerWPF.ValueConverters
{
    public class CultureToImageConverter : BaseValueConverter<CultureToImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cal = value as CultureInfo;
            if (cal == null) return null;
            //var imageBase64 = 
            return cal.GetLangFlagImage();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
