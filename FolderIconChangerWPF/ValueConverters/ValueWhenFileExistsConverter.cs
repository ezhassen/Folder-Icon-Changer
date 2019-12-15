using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;

namespace FolderIconChangerWPF.ValueConverters
{
    public abstract class ValueWhenFileExistsConverterBase<ValueType> : ValueWhenConverterBase<bool, ValueType>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();
            try
            {
                string strValue = value as string;
                if (string.IsNullOrEmpty(strValue)) strValue = "";
                if (File.Exists(strValue) == When) return Value;

                return Otherwise;
            }
            catch
            {
                return Otherwise;
            }
        }
    }

    public class BoolWhenFileExistsConverter : ValueWhenFileExistsConverterBase<bool>
    {
        public BoolWhenFileExistsConverter()
        {
            When = true;
            Value = true;
            Otherwise = false;
        }

    }
    public class VisibiltyWhenFileExistsConverter : ValueWhenFileExistsConverterBase<Visibility>
    {
        public VisibiltyWhenFileExistsConverter()
        {
            When = true;
            Value = Visibility.Visible;
            Otherwise = Visibility.Hidden;
        }
    }

}
