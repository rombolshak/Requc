using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Requc.Models;

namespace Requc.Converters
{
    class EvaToTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)values[1])
            {
                switch ((MeasurementResult)values[0])
                {
                    case MeasurementResult.Phase0:
                        return "1";
                    case MeasurementResult.Phase1:
                        return "0";
                    case MeasurementResult.Inconclusive:
                        return "?";
                }
            }

            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
