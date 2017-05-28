using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BikeTouringGIS.Converters
{
    public class SplitIsEnabledConverter : IMultiValueConverter
    {
        // [0] = issplitted moet false zijn
        // [1] = lengte split stuk in configuratie
        // [2] = lengte van geselecteerde route, moet dus groter zijn dan [1]
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return false;
            }
            var isSplitted = (bool)values[0];
            var lengthOfASplitPart = (int)values[1];
            var lengthOfRoute = (int)values[2];
            return isSplitted == false && lengthOfASplitPart < lengthOfRoute;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}