using BikeTouringGIS.Controls;
using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;
using Fluent;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BikeTouringGIS.ViewModels
{
    public class LayerListViewModel : BikeTouringGISBaseViewModel
    {
        private ObservableCollection<BikeTouringGISLayer> _layers;
        private BikeTouringGISLayer _selectedLayer;

        public ObservableCollection<BikeTouringGISLayer> Layers
        {
            get { return _layers; }
            set
            {
                Set(ref _layers, value);
                if(_layers != null && SelectedLayer == null)
                {
                    SelectedLayer = _layers.Where(x => x.Type == LayerType.GPXRoutes).FirstOrDefault();
                }
            }
        }

        public BikeTouringGISLayer SelectedLayer
        {
            get { return _selectedLayer; }
            set { Set(ref _selectedLayer, value); }
        }
    }
}