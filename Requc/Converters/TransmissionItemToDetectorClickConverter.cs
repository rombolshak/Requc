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
            var catchedByEva = (bool) values[2];
            var isBlocked = (bool) values[3];
            return alice == bob && !catchedByEva || isBlocked ? "-" : "+";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}