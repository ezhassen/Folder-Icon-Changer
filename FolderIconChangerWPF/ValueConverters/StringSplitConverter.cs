using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderIconChangerWPF.ValueConverters
{
    public class StringSplitConverter : BaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();
            var val = value as string;
            if (string.IsNullOrEmpty(val)) return null;
            var sp = val.Split(new string[] { Seperator }, StringSplitOptions.None);

            if (sp.Length == 0 || Index >= sp.Length) return null;
            var res = sp[Index];
            var format = (parameter as string) ?? Format;
            if (!string.IsNullOrEmpty(format))
            {
                //if (culture == null)
                //{
                //    return string.Format(Format, value);
                //}

                //try
                //{
                //    return string.Format(culture, format, value);
                //}
                //catch
                //{
                res = string.Format(format, res);
                //}
            }
            return res;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        public string Seperator { get; set; } = ";";
        public string Format { get; set; }
        public int Index { get; set; } = 0;
    }
}
