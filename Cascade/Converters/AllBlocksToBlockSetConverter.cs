using System;
using System.Globalization;
using System.Windows.Data;

namespace Cascade.Converters
{
    class AllBlocksToBlockSetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var objects = (object[]) value;
            return objects[Int32.Parse(parameter.ToString())];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
