using BikeTouringGIS.Controls;
using Esri.ArcGISRuntime.Controls;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGIS.Converters
{
    class LayerLoadedEventArgsToBikeTouringGISLayerConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var args = (LayerLoadedEventArgs)value;
            var bikeTouringLayer = args.Layer as BikeTouringGISLayer;
            return bikeTouringLayer;
        }
    }
}
