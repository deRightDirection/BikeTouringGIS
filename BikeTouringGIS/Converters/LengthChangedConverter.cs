using BikeTouringGIS.Controls;
using BikeTouringGIS.Models;
using GalaSoft.MvvmLight.Command;
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
    class LengthChangedConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var distance = (RoutedPropertyChangedEventArgs<double>)value;
            var newDistance = System.Convert.ToInt32(distance.NewValue);
            var layer = (BikeTouringGISLayer)parameter;
            return new SplitLayerProperties() { Layer = layer, Distance = newDistance };
        }
    }
}
