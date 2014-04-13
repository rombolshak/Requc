using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Requc.Converters
{
    public class PhaseValueToImageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var uri = (double) values[0] - (double) values[1] < 1e-5
                          ? new Uri("/Resources/phi0.gif", UriKind.Relative)
                          : new Uri("/Resources/phi1.gif", UriKind.Relative);
            return new BitmapImage(uri);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}