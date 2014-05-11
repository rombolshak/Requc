using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Requc.Models;

namespace Requc.Converters
{
    public class ValuesToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var objects = parameter as object[];
            var cathced = (bool) values[2];
            var aliceValue = (bool) values[0];
            var bobValue = (bool)values[1];
            if (cathced)
            {
                return objects[2];
            }

            return aliceValue == bobValue ? objects[0] : objects[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}