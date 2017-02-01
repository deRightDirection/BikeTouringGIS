using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BikeTouringGIS.Converters
{
    class LengthToMaximumSplitLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var length = (int)value;
            var newLength = Math.Floor((double)(length / 10)) * 10;
            if(newLength > 150)
            {
                newLength = 150;
            }
            return newLength;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}