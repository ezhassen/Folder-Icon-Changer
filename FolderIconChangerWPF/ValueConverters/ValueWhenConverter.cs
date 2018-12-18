using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace FolderIconChangerWPF.ValueConverters
{

    /*Usage
     * https://github.com/Windows-XAML/Template10/wiki/Converters#changetypeconverter
Implementation
xmlns:s="clr-namespace:System;assembly=mscorlib"

You can add this binding as a Resource to another XAML element.

<converters:ValueWhenConverter x:Key="VisibleWhenTrueConverter">
    <converters:ValueWhenConverter.When>
        <s:Boolean>True</s:Boolean>
    </converters:ValueWhenConverter.When>
    <converters:ValueWhenConverter.Value>
        <Visibility>Visible</Visibility>
    </converters:ValueWhenConverter.Value>
    <converters:ValueWhenConverter.Otherwise>
        <Visibility>Collapsed</Visibility>
    </converters:ValueWhenConverter.Otherwise>
</converters:ValueWhenConverter>
With the resource in place, you can use the resource as the Converter when binding a value on your page.

<TextBlock Text="Hello Admin" Visibility="{Binding IsAdmin, Converter={StaticResource VisibleWhenTrueConverter}}" />
Note, if you want to use a value of Null, use the following syntax:

<converters:ValueWhenConverter x:Key="VisibleWhenNullConverter" When="{x:Null}">
	<converters:ValueWhenConverter.Value>
		<Visibility>Visible</Visibility>
	</converters:ValueWhenConverter.Value>
	<converters:ValueWhenConverter.Otherwise>
		<Visibility>Collapsed</Visibility>
	</converters:ValueWhenConverter.Otherwise>
</converters:ValueWhenConverter>

         */
    /// <summary>
    /// This converter can display data from one of two binary choices. If the data being bound is equivalent to the When property of the converter, then the result of the binding will be the Value property of the converter. If they are not equivalent, the result of the binding will be the Otherwise converter.
    /// </summary>
    public abstract class ValueWhenConverterBase<WhenType, ValueType> : BaseValueConverter
    {
        /// <summary>
        /// This object is the result of the binding conversion if the originally bound value is equivalent to the value of the When property.
        /// </summary>
        public virtual ValueType Value { get; set; } = default(ValueType);
        /// <summary>
        /// This object is the result of the binding conversion if the originally bound value is NOT equivalent to the value of the When property.
        /// </summary>
        public virtual ValueType Otherwise { get; set; } = default(ValueType);
        /// <summary>
        /// This is the object evaluated for equivalence with the bound value. The bound value is technically an input parameter to this converter.
        /// </summary>
        public virtual WhenType When { get; set; } = default(WhenType);
        public virtual WhenType OtherwiseValueBack { get; set; } = default(WhenType);

        //public IList MultiWhen { get; set; }

        /// <summary>
        /// Checks if any value in object[] values contains defined MultiWhen values.
        /// Otherwise All object[] values must be in MultiWhen.
        /// </summary>
        public bool MultiWhenAny { get; set; }

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

        //
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();

            try
            {
                if (values is null) return Otherwise;
                if (When is IList multiWhen)
                {
                    if (MultiWhenAny)
                    {
                        if (values.Any(val => multiWhen.Contains(val))) return Value;

                        //foreach (var val in values)
                        //{
                        //    if (MultiWhen.Contains(val)) return Value;
                        //}
                        return Otherwise;
                    }
                    else //And
                    {
                        if (values.All(val => multiWhen.Contains(val))) return Value;

                        //foreach (var val in values)
                        //{
                        //    if (!MultiWhen.Contains(val)) return Otherwise;
                        //}
                        return Value;
                    }
                }
                else
                {
                    var whenObj = When as object;
                    if (MultiWhenAny)
                    {
                        if (values.Any(val => val.Equals(When))) return Value;
                    }
                    else
                    {
                        if (values.All(val => val.Equals(When))) return Value;
                    }

                    return Otherwise;
                }

            }
            catch
            {
                return Otherwise;
            }

        }


    }
    public class ValueWhenConverter : ValueWhenConverterBase<object, object> { }
    public class ValueWhenBoolConverter : ValueWhenConverterBase<bool, object>
    {
        public ValueWhenBoolConverter()
        {
            When = true;
        }
    }
    public class ValueWhenBoolConverterInt : ValueWhenConverterBase<bool, int>
    {
        public ValueWhenBoolConverterInt()
        {
            When = true;
        }
    }
    public class ValueWhenNullConverter : ValueWhenConverterBase<object, object> { }

    public class BoolWhenBoolConverter : ValueWhenConverterBase<bool, bool>
    {
        public BoolWhenBoolConverter()
        {
            When = true;
            Value = true;
            Otherwise = false;
        }
    }
    public class BoolWhenConverter : ValueWhenConverterBase<object, bool>
    {
        public BoolWhenConverter()
        {
            Value = true;
            Otherwise = false;
        }
    }
    public class BoolWhenIntConverter : ValueWhenConverterBase<int, bool>
    {
        public BoolWhenIntConverter()
        {
            Value = true;
            Otherwise = false;
        }
    }

    public class BoolWhenNullConverter : ValueWhenConverterBase<object, bool>
    {
        public BoolWhenNullConverter()
        {
            Value = false;
            Otherwise = true;
        }
    }

    public class ValueWhenIntConverter : ValueWhenConverterBase<int, object>
    {
        public ValueWhenIntConverter()
        {
            Value = true;
            Otherwise = false;
        }
    }
    public class FalseWhenZeroConverter : ValueWhenConverterBase<int, bool>
    {
        public FalseWhenZeroConverter()
        {
            this.When = 0;
            this.Value = false;
            this.Otherwise = true;
        }
    }
    public class TrueWhenZeroConverter : ValueWhenConverterBase<int, bool>
    {
        public TrueWhenZeroConverter()
        {
            this.When = 0;
            this.Value = true;
            this.Otherwise = false;
        }
    }
}
