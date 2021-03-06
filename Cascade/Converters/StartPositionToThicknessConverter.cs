﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Cascade.Converters
{
    class StartPositionToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var startPosition = (int) value;
            return new Thickness(0, startPosition*20, 0, 2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
