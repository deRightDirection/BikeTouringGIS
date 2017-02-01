using BikeTouringGIS.Controls;
using BikeTouringGIS.Models;
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
    public class MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var newDistance = System.Convert.ToInt32(values[1]);
            var layer = (BikeTouringGISLayer)values[0];
            return new SplitLayerProperties() { Layer = layer, Distance = newDistance };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
