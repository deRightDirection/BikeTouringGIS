using BikeTouringGIS.Controls;
using Esri.ArcGISRuntime.Controls;
using GalaSoft.MvvmLight.Command;

namespace BikeTouringGIS.Converters
{
    internal class LayerLoadedEventArgsToBikeTouringGISLayerConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var args = (LayerLoadedEventArgs)value;
            var bikeTouringLayer = args.Layer as BikeTouringGISLayer;
            return bikeTouringLayer;
        }
    }
}