using System;
using System.Globalization;
using System.Windows.Data;
using Requc.Commands;
using Requc.Helpers;

namespace Requc.Converters
{
    public class EnumValueToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumHelpers.GetEnumDescription((Enum) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumHelpers.GetValueFromDescription<ModelingMode>((string) value);
        }
    }
}