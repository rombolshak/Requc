using System;
using System.Globalization;
using System.Windows.Data;

namespace Cascade.Converters
{
    public class WorkingSizeToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 10*(int) value - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}