using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FolderIconChangerWPF.ValueConverters
{
    /*Usage
     Formatting Types in .NET: https://msdn.microsoft.com/en-us/library/26etazsy.aspx
Standard Date/Time Format Strings: http://msdn.microsoft.com/en-us/library/az4se3k1.aspx
Custom Date/Time Format Strings: http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx
Standard Numeric Format Strings: http://msdn.microsoft.com/en-us/library/dwhawy9k.aspx
Custom Numeric Format Strings: http://msdn.microsoft.com/en-us/library/0c899ak8.aspx
Implementation
You can add this control as a Resource to another XAML element:

<Page.Resources>
    <converters:StringFormatConverter x:Key="StrFormatConverter" />
    <converters:StringFormatConverter x:Key="PriceConverter" Format="{}{0:N4}"/>
    <converters:StringFormatConverter x:Key="ValueConverter" Format="{}{0:N2}"/>
</Page.Resources>
With the resource in place, you can use the resource as the Converter when binding a value on your page. The ConverterParameter binding property specifies the format string:

<TextBlock Text="{Binding DateTimeValue, Converter={StaticResource StrFormatConverter}, ConverterParameter=\{0:D\}}" />
<TextBlock Text="{Binding PriceProperty, Converter={StaticResource PriceConverter}" />
<TextBlock Text="{Binding ValueProperty, Converter={StaticResource ValueConverter}" />
         
         
         */
    /// <summary>
    /// This converter can take in a value and format it using format strings provided as either a parameter or property of the converter.
    /// </summary>
    public class StringFormatConverter : BaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = (parameter as string) ?? Format;
            if (format == null)
            {
                return value;
            }

            if (culture == null)
            {
                return string.Format(format, value);
            }

            try
            {
                return string.Format(culture, format, value);
            }
            catch
            {
                return string.Format(format, value);
            }
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        //
        public string Format { get; set; }
    }

    public class ObjectToStringConverter : BaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DefaultValue;
            var format = Format;
            if (format == null) return value.ToString();

            if (value is decimal dec)  return dec.ToString(format);
            if (value is int integer)  return integer.ToString(format);
            if (value is double doubl)  return doubl.ToString(format);
            if (value is float floa)  return floa.ToString(format);
            //
            if (value is DateTime datet)  return datet.ToString(format);
            
            return value.ToString();
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        //
        public string Format { get; set; }
        public string DefaultValue { get; set; }
    }


}
