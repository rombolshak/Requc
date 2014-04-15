namespace Requc.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Models;

    public class TransmissionItemToDetectorClickConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var alice = (bool)values[0];
            var bob = (bool)values[1];
            return alice == bob ? "-" : "+";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}