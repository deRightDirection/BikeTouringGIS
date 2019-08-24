using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using theRightDirection.Library;

namespace BikeTouringGIS.Converters
{
    public class NotNullAndPrefixConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSplitted = false;
            bool hasPrefix = false;
            if (values[0] != DependencyProperty.UnsetValue)
            {
                isSplitted = (bool)values[0];
            }
            if (values[1] != DependencyProperty.UnsetValue)
            {
                if (values[1] != null)
                {
                    var valueToCheck = ((string)values[1]);
                    hasPrefix = valueToCheck.Length > 0;
                    hasPrefix = valueToCheck.IsValidFileName();
                }
            }
            return isSplitted && hasPrefix;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}