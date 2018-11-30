using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FolderIconChangerWPF.ValueConverters
{
    public abstract class ValueWhenStringConverterBase<ValueType> : ValueWhenConverterBase<string, ValueType>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();
            try
            {
                string strValue = value as string;
                if (string.IsNullOrEmpty(strValue)) strValue = "";
                string strWhen = When as string;
                if (string.IsNullOrEmpty(strWhen)) strWhen = "";
                if ((parameter as bool?) ?? IgnoreCase)
                {
                    if (strValue.Equals(strWhen, StringComparison.OrdinalIgnoreCase)) return Value;
                }
                else
                {
                    if (strValue.Equals(strWhen, StringComparison.Ordinal)) return Value;
                }

                return Otherwise;
            }
            catch
            {
                return Otherwise;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();

            if (OtherwiseValueBack == null) throw new InvalidOperationException("Cannot ConvertBack if no OtherwiseValueBack is set!");

            try
            {
                if (object.Equals(value, Value)) return When;

                return OtherwiseValueBack;
            }
            catch
            {
                return OtherwiseValueBack;
            }
        }

        public bool IgnoreCase { get; set; } = true;
    }

    public class ValueWhenStringConverter : ValueWhenStringConverterBase<ValueWhenStringConverter> { }
}
