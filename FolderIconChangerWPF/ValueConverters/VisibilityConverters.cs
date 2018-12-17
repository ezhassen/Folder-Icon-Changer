using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace FolderIconChangerWPF.ValueConverters
{

    public abstract class VisibilityWhenConverterBase<WhenType> : ValueWhenConverterBase<WhenType, Visibility>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();
            try
            {
                if (object.Equals(value, parameter ?? When))
                {
                    return Value;
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
        /// <summary>
        /// This object is the result of the binding conversion if the originally bound value is equivalent to the value of the When property.
        /// </summary>
        [DefaultValue(Visibility.Visible)]
        public override Visibility Value { get; set; } = Visibility.Visible;

        /// <summary>
        /// This object is the result of the binding conversion if the originally bound value is NOT equivalent to the value of the When property.
        /// </summary>
        [DefaultValue(Visibility.Hidden)]
        public override Visibility Otherwise { get; set; } = Visibility.Hidden;
    }

    public class VisibilityWhenConverter : VisibilityWhenConverterBase<object> { }
    public class VisibilityWhenVisibilityConverter : VisibilityWhenConverterBase<Visibility> {
        public VisibilityWhenVisibilityConverter()
        {
            When = Visibility.Visible;
            Value = Visibility.Visible;
            Otherwise = Visibility.Hidden;
        }

    }
    //Boolean
    public class VisibilityWhenBoolConverter : VisibilityWhenConverterBase<bool>
    {
        public VisibilityWhenBoolConverter()
        {
            this.When = true;
            //this.Value = Visibility.Visible;
            //this.Otherwise = Visibility.Hidden;
        }
    }
    public class VisibilityWhenTrueConverter : VisibilityWhenBoolConverter
    {
        public VisibilityWhenTrueConverter()
        {
            this.When = true;
            this.Value = Visibility.Visible;
            this.Otherwise = Visibility.Hidden;
        }
    }
    public class VisibilityWhenFalseConverter : VisibilityWhenBoolConverter
    {
        public VisibilityWhenFalseConverter()
        {
            this.When = false;
            this.Value = Visibility.Visible;
            this.Otherwise = Visibility.Hidden;
        }
    }
    //
    public class VisibilityWhenIntConverter : VisibilityWhenConverterBase<int> { }
    //Null
    public class VisibilityWhenNullConverter : ValueWhenConverterBase<object, Visibility>
    {
        public VisibilityWhenNullConverter()
        {
            this.When = null;
            this.Value = Visibility.Visible;
            this.Otherwise = Visibility.Hidden;
        }
    }
    public class VisibilityWhenNotNullConverter : ValueWhenConverterBase<object, Visibility>
    {
        public VisibilityWhenNotNullConverter()
        {
            this.When = null;
            this.Value = Visibility.Hidden;
            this.Otherwise = Visibility.Visible;
        }
    }
    //string
    public class VisibilityWhenStringConverter : ValueWhenStringConverterBase<Visibility> { }

    public class ValueWhenVisibilityConverter : ValueWhenConverterBase<Visibility, object> { }
    public class BoolWhenVisibilityConverter : ValueWhenConverterBase<Visibility, bool>
    {
        public BoolWhenVisibilityConverter()
        {
            this.When = Visibility.Visible;
            this.Value = true;
        }
    }

}
