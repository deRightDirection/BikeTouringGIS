using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BikeTouringGIS.Converters
{
    public class StringToIntConverter : DependencyObject, IValueConverter
    {
        private int _originalValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _originalValue = (int)value;
            return _originalValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int newValue;
            if (int.TryParse(value.ToString(), out newValue))
            {
                if (newValue < 0)
                {
                    newValue = 0;
                }
                if (newValue > 125)
                {
                    newValue = 125;
                }
                return newValue;
            }
            return _originalValue;
        }
    }
}