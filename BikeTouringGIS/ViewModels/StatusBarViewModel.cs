using Esri.ArcGISRuntime.Controls;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BikeTouringGIS.ViewModels
{
    public class StatusBarViewModel : ViewModelBase
    {
        private MapView _mapView;
        private double _zoom;
        public StatusBarViewModel()
        {
            _zoom = 0;
        }
        public MapView MapView
        {
            set { Set(ref _mapView, value); }
        }

        public double Zoom
        {
            get { return _zoom; }
            set
            {
                Set(ref _zoom, value);
                ZoomMap();
            }
        }

        private void ZoomMap()
        {
            _mapView.ZoomAsync(Zoom);
        }
    }
}