using BikeTouringGIS.Controls;
using BikeTouringGIS.Models;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace BikeTouringGIS.Converters
{
    internal class LengthChangedConverter : IEventArgsConverter
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