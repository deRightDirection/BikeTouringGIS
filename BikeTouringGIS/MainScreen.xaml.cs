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
        private List<wptType> _routePoints;
        private string _lastUsedFolder;
        private GraphicsLayer _poiLayer;
        private string _originalFile;
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

        private void SaveGPSTracks_Click(object sender, RoutedEventArgs e)
        {
            var pois = GetPOIs();
            // TODO pois bufferen om route
            /*
                for (int i = 0; i < _routeParts.Count; i++)
                {
                    var filename = string.Format(@"{0}\{1}_{2}.gpx", _lastUsedFolder, prefix.Text, i + 1);
                    var gpxFile = new GPXFile();
                    var gpx = new gpxType();
                    var rte = new rteType();
                    rte.name = $"{i + 1}_{prefix.Text}";
                    rte.rtept = _routeParts[i].ToArray();
                    gpx.rte = new List<rteType>() { rte }.ToArray();
                    gpx.wpt = pois;
                    gpxFile.Save(filename, gpx);
                }
                */
            SaveAllSplitPointsAndLinesInOneFile();
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

        private void SaveAllSplitPointsAndLinesInOneFile()
        {
            var filename = string.Format(@"{0}\{2}_splitlines_and_points_{1}_km.gpx", _lastUsedFolder);
            var gpxFile = new GPXFile();
            var gpx = new gpxType();
            gpx.wpt = _routePoints.ToArray();
            var routes = new List<rteType>();
            /*
            for (int i = 0; i < _routeParts.Count; i++)
            {
                var rte = new rteType();
                rte.name = string.Format("{0}_{1}", prefix.Text, i + 1);
                rte.rtept = _routeParts[i].ToArray();
                routes.Add(rte);
            }
            */
            gpx.rte = routes.ToArray();
            gpxFile.Save(filename, gpx);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var pois = GetPOIs();
            var filename = _originalFile;
            var gpxFile = new GPXFile();
            var gpx = new gpxType();
            var rte = new rteType();
//            rte.rtept = _wayPoints.ToArray();
            gpx.rte = new List<rteType>() { rte }.ToArray();
            gpx.wpt = pois;
            gpxFile.Save(filename, gpx);
        }
    }
}
