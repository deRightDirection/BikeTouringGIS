using BikeTouringGISLibrary.Enumerations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WinUX.Common;

namespace BikeTouringGIS.Converters
{ 
    public class LayerTypeToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = value.ToString();
            var stringValue = ParseHelper.SafeParseEnum<LayerType>(enumValue);
            if (((string)parameter).Equals("Color"))
            {
                switch (stringValue)
                {
                    case LayerType.GPXRoute:
                        return new SolidColorBrush(Colors.LimeGreen);
                    case LayerType.GPXTrack:
                        return new SolidColorBrush(Colors.Pink);
                    case LayerType.PointsOfInterest:
                        return new SolidColorBrush(Colors.White);
                    default:
                        return new SolidColorBrush(Colors.NavajoWhite);
                }
            }
            if (((string)parameter).Equals("Text"))
            {
                switch (stringValue)
                {
                    case LayerType.GPXRoute:
                        return "R";
                    case LayerType.GPXTrack:
                        return "T";
                    case LayerType.PointsOfInterest:
                        return "P";
                    default:
                        return "U";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
