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
    public class NotNullAndPrefixConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSplitted = false;
            bool hasPrefix = false;
            if(values[0] != DependencyProperty.UnsetValue)
            {
                isSplitted = (bool)values[0];
            }
            if (values[1] != DependencyProperty.UnsetValue)
            {
                hasPrefix = ((string)values[1]).Length > 0;
            }
            return isSplitted && hasPrefix;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
