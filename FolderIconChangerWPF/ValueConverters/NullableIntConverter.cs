using System;

namespace FolderIconChangerWPF.ValueConverters
{
    public class NullableIntConverter : ValueWhenConverterBase<object, int>
    {
        public bool UseMaxIntWhenNull { get; set; } = true;
        public int? CustomValueWhenNull { get; set; }


        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue;
            }
            else if (value == null)
            {
                if (UseMaxIntWhenNull) return int.MaxValue;
                if (CustomValueWhenNull.HasValue) return CustomValueWhenNull.Value;
                return 0;
            }
            else
            {
                throw new InvalidCastException("Value must be an int or null.");
            }
        }
    }
}
