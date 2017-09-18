using BikeTouringGISApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace BikeTouringGISApp.Converters
{
    public class ObjectToLogBookConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                LogBook alert = (LogBook)value;
                return alert;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                return value;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}