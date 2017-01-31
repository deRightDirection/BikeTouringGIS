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
        private GraphicsLayer _routelayer, _poiLayer;
        private GraphicsLayer _splitLayer;
        private List<Route> _routes;
        private string _originalFile;
        public MainScreen()
        {
            InitializeComponent();
            _routes = new List<Route>();
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

        private void DisplayPartOfTrack(List<wptType> wayPoints, Color color)
        {
            /*
            var lineSymbol = new SimpleLineSymbol();
            lineSymbol.Width = 4;
            lineSymbol.Color = color;
            var geometry = CreateGeometryFromWayPoints(wayPoints);
            var track = new Graphic(geometry, lineSymbol);
            _splitLayer.Graphics.Add(track);
            */
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
            var filename = string.Format(@"{0}\{2}_splitlines_and_points_{1}_km.gpx", _lastUsedFolder, distanceTxt.Text, prefix.Text);
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

        private void CreateSplitPoints_Click(object sender, RoutedEventArgs e)
        {
            if (_routelayer.Graphics.Count == 3)
            {
                _routelayer.Graphics.RemoveAt(0);
            }
            _splitLayer.Graphics.Clear();
            _routePoints = new List<wptType>();
            //// splits de route op naar stukken van x km
            //var routeSplitter = new RouteSplitter(_wayPoints);
            //routeSplitter.SplitRoute(int.Parse(distanceTxt.Text));
            //// collectie van routes en verschillende kleuren
            //var x = routeSplitter.SplittedRoutes;
            //_routeParts = x;
            //var items = ColorUtils.RgbLinearInterpolate(Colors.LightBlue, Colors.DarkBlue, x.Count, Colors.LightBlue, Colors.DarkBlue);
            //// doorloop alle routes, teken iedere route op de kaart zet voor ieder beginpunt een stuk op de kaart plus afstand tot dit punt
            //int distance = 0;
            //for (int j = 0; j < x.Count; j++)
            //{
            //    DisplayPartOfTrack(x[j], items[j]);
            //    if (j > 0)
            //    {
            //        var point = x[j][0];
            //        var g = new Graphic(new MapPoint((double)point.lon, (double)point.lat, new SpatialReference(4326)), grid.Resources["RouteSymbol"] as SimpleMarkerSymbol);
            //        g.Attributes["index"] = distance;
            //        point.name = distance.ToString();
            //        _routePoints.Add(point);
            //        _splitLayer.Graphics.Add(g);
            //    }
            //    distance += _distanceAnalyzer.CalculateDistance(x[j]);
            //}

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
