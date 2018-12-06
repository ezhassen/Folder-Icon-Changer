using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FolderIconChangerWPF.ValueConverters
{
    public abstract class ValueWhenFolderExistsConverterBase<ValueType> : ValueWhenConverterBase<bool, ValueType>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();
            try
            {
                string strValue = value as string;
                if (string.IsNullOrEmpty(strValue)) strValue = "";
                if (Directory.Exists(strValue) == When) return Value;

                return Otherwise;
            }
            catch
            {
                return Otherwise;
            }
        }
    }

    public class BoolWhenFolderExistsConverter : ValueWhenFolderExistsConverterBase<bool>
    {
        public BoolWhenFolderExistsConverter()
        {
            When = true;
            Value = true;
            Otherwise = false;
        }

    }
    public class VisibiltyWhenFolderExistsConverter : ValueWhenFolderExistsConverterBase<Visibility>
    {
        public VisibiltyWhenFolderExistsConverter()
        {
            When = true;
            Value = Visibility.Visible;
            Otherwise = Visibility.Hidden;
        }
    }

}
