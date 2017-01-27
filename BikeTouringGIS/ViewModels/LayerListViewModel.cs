using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;
using Fluent;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BikeTouringGIS.ViewModels
{
    public class LayerListViewModel : BikeTouringGISBaseViewModel
    {
        public LayerCollection Layer
        {
            get { return _map.Layers; }
        }
    }
}