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

namespace BikeTouringGIS
{
    /// <summary>
    /// Interaction logic for MainScreen.xaml
    /// </summary>
    public partial class MainScreen : UserControl
    {
        private List<List<wptType>> _routeParts;
        private List<wptType> _routePoints;
        private string _lastUsedFolder;
        private GraphicsLayer _routelayer;
        private GraphicsLayer _splitLayer;
        private TrackReader _trackReader;
        private DistanceAnalyzer _distanceAnalyzer;
        public MainScreen()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "GPX files (*.gpx)|*.gpx";
            _routelayer = Map.Layers["route"] as GraphicsLayer;
            _splitLayer = Map.Layers["split"] as GraphicsLayer;
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = DropBoxHelper.GetDropBoxFolder();
            if (openFileDialog.ShowDialog() == true)
            {
                _routelayer.Graphics.Clear();
                _splitLayer.Graphics.Clear();
                _lastUsedFolder = Path.GetDirectoryName(openFileDialog.FileName);
                _trackReader = new TrackReader(openFileDialog.FileName);
                // lees gpx tracks en zoom in naar gehele route
                _trackReader.ReadGPXTrack();
                ZoomToRoute(_trackReader.WayPoints);
                // bepaal afstand gehele route
                _distanceAnalyzer = new DistanceAnalyzer();
                length.Text = _distanceAnalyzer.CalculateDistance(_trackReader.WayPoints).ToString();
                // teken begin en eind punt op de kaart
                GetStartAndEndPoint(grid.Resources["StartSymbol"] as SimpleMarkerSymbol, grid.Resources["EndSymbol"] as SimpleMarkerSymbol, _trackReader.WayPoints);
            }
        }

        private void ZoomToRoute(List<wptType> wayPoints)
        {
            var geometry = CreateGeometryFromWayPoints(wayPoints);
            MyMapView.SetView(geometry.Extent.Expand(1.2));
        }

        private void GetStartAndEndPoint(SimpleMarkerSymbol startSymbol, SimpleMarkerSymbol endSymbol, List<wptType> wayPoints)
        {
            var geometry = CreateGeometryFromWayPoints(wayPoints);
            var track = new Graphic(geometry, grid.Resources["TotalRoute"] as SimpleLineSymbol);
            _routelayer.Graphics.Add(track);
            _routelayer.Graphics.Add(new Graphic(new MapPoint((double)wayPoints.First().lon, (double)wayPoints.First().lat, new SpatialReference(4326)), startSymbol));
            _routelayer.Graphics.Add(new Graphic(new MapPoint((double)wayPoints.Last().lon, (double)wayPoints.Last().lat, new SpatialReference(4326)), endSymbol));
        }

        private void DisplayPartOfTrack(List<wptType> wayPoints, Color color)
        {
            var lineSymbol = new SimpleLineSymbol();
            lineSymbol.Width = 3;
            lineSymbol.Color = color;
            var geometry = CreateGeometryFromWayPoints(wayPoints);
            var track = new Graphic(geometry, lineSymbol);
            _splitLayer.Graphics.Add(track);
        }

        private Polyline CreateGeometryFromWayPoints(List<wptType> wayPoints)
        {
            var builder = new PolylineBuilder(new SpatialReference(4326));
            foreach (var wayPoint in wayPoints)
            {
                builder.AddPoint(new MapPoint((double)wayPoint.lon, (double)wayPoint.lat));
            }
            return builder.ToGeometry();
        }

        private void SaveGPSTracks_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _routeParts.Count; i++)
            {
                var filename = string.Format(@"{0}\{1}_{2}.gpx", _lastUsedFolder, prefix.Text, i + 1);
                var gpxFile = new GPXFile();
                var gpx = new gpxType();
                var rte = new rteType();
                rte.name = string.Format("{0}_{1}", prefix.Text, i + 1);
                rte.rtept = _routeParts[i].ToArray();
                gpx.rte = new List<rteType>() { rte }.ToArray();
                gpxFile.Save(filename, gpx);
            }
            SaveAllSplitPointsAndLinesInOneFile();
        }

        private void SaveAllSplitPointsAndLinesInOneFile()
        {
            var filename = string.Format(@"{0}\splitlines_and_points_{1}_km.gpx", _lastUsedFolder, distanceTxt.Text);
            var gpxFile = new GPXFile();
            var gpx = new gpxType();
            gpx.wpt = _routePoints.ToArray();
            var routes = new List<rteType>();
            for (int i = 0; i < _routeParts.Count; i++)
            {
                var rte = new rteType();
                rte.name = string.Format("{0}_{1}", prefix.Text, i + 1);
                rte.rtept = _routeParts[i].ToArray();
                routes.Add(rte);
            }
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
            // splits de route op naar stukken van x km
            var routeSplitter = new RouteSplitter(_trackReader.WayPoints);
            routeSplitter.SplitRoute(int.Parse(distanceTxt.Text));
            // collectie van routes en verschillende kleuren
            var x = routeSplitter.SplittedRoutes;
            _routeParts = x;
            var items = ColorUtils.RgbLinearInterpolate(Colors.Gold, Colors.YellowGreen, Colors.LimeGreen, x.Count);
            var differenceCount = x.Count - items.Count;
            for(int i = 0; i < differenceCount; i++)
            {
                items.Add(Colors.YellowGreen);
            }


            // doorloop alle routes, teken iedere route op de kaart zet voor ieder beginpunt een stuk op de kaart plus afstand tot dit punt
            int distance = 0;
            for (int j = 0; j < x.Count; j++)
            {
                DisplayPartOfTrack(x[j], items[j]);
                if (j > 0)
                {
                    var point = x[j][0];
                    var g = new Graphic(new MapPoint((double)point.lon, (double)point.lat, new SpatialReference(4326)), grid.Resources["RouteSymbol"] as SimpleMarkerSymbol);
                    g.Attributes["index"] = distance;
                    point.name = distance.ToString();
                    _routePoints.Add(point);
                    _splitLayer.Graphics.Add(g);
                }
                distance += _distanceAnalyzer.CalculateDistance(x[j]);
            }

        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
