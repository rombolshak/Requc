﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Cascade.Converters
{
    class BlockSizeToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var size = (int) value;
            return size*20 - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
