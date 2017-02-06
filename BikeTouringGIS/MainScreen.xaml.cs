using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using GPX;
using Squirrel;
using BicycleTripsPreparationApp;
using theRightDirection.Library.Logging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using BikeTouringGISLibrary;
using BikeTouringGISLibrary.Model;

namespace BikeTouringGIS
{
    /// <summary>
    /// Interaction logic for MainScreen.xaml
    /// </summary>
    public partial class MainScreen : UserControl
    {
        private GraphicsLayer _poiLayer;
        public MainScreen()
        {
            InitializeComponent();
        }

        private void ShowWayPoints(List<wptType> waypoints)
        {
            var poiSymbol = this.Resources["POISymbol"] as SimpleMarkerSymbol;
            foreach (var point in waypoints)
            {
                var mapPoint = new Graphic(new MapPoint((double)point.lon, (double)point.lat, new SpatialReference(4326)), poiSymbol);
                _poiLayer.Graphics.Add(mapPoint);
            }
        }

        private wptType[] GetPOIs()
        {
            var pois = _poiLayer.Graphics;
            var result = new List<wptType>();
            foreach(var poi in pois)
            {
                var wpt = new wptType();
                var point = poi.Geometry as MapPoint;
                wpt.lon = (decimal)point.X;
                wpt.lat = (decimal)point.Y;
                wpt.name = poi.Attributes["title"] as string;
                wpt.desc = poi.Attributes["title"] as string;
                result.Add(wpt);
            }
            return result.ToArray();
        }
    }
}
