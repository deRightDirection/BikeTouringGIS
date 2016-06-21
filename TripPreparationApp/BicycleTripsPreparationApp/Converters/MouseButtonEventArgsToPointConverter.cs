using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BikeTouringGIS.Converters
{
    class MouseButtonEventArgsToPointConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var args = (MouseButtonEventArgs)value;
            var element = (FrameworkElement)parameter;
            var point = args.GetPosition(element);
            var mapView = parameter as MapView;
            var mapPoint = mapView.ScreenToLocation(point);
            mapPoint = GeometryEngine.NormalizeCentralMeridian(mapPoint) as MapPoint;
            mapPoint = (GeometryEngine.Project(mapPoint, new SpatialReference(4326))) as MapPoint;
            return mapPoint;
        }
    }
}
