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
        private GraphicsLayer _routelayer, _poiLayer;
        private GraphicsLayer _splitLayer;
        private List<wptType> _wayPoints;
        private DistanceAnalyzer _distanceAnalyzer;
        private string _originalFile;
        public MainScreen()
        {
            InitializeComponent();
            var versionApp = typeof(App).Assembly.GetName().Version;
            version.Text = string.Format("version {0}.{1}.{2}", versionApp.Major, versionApp.Minor,versionApp.Build);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "GPX files (*.gpx)|*.gpx";
            _routelayer = Map.Layers["route"] as GraphicsLayer;
            _splitLayer = Map.Layers["split"] as GraphicsLayer;
            _poiLayer = Map.Layers["POIs"] as GraphicsLayer;
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = DropBoxHelper.GetDropBoxFolder();
            if (openFileDialog.ShowDialog() == true)
            {
                _routelayer.Graphics.Clear();
                _splitLayer.Graphics.Clear();
                _poiLayer.Graphics.Clear();
                _originalFile = openFileDialog.FileName;
                _lastUsedFolder = Path.GetDirectoryName(_originalFile);
                GetWayPoints(openFileDialog.FileName);
            }
        }

        private async void GetWayPoints(string fileName)
        {
            var gpx = new GPXFile(fileName);
            var waypoints = gpx.GetWaypoints();
            var routes = gpx.GetRoutes();
            var tracks = gpx.GetTracks();
            // TODO MME 02062016: what if there are more routes & tracks?
            if (routes.Count == 1)
            {
                var route = gpx.GetRoutes()[0];
                _wayPoints = route.RouteWayPoints;
                SetRoute();
            }
            if(routes.Count == 0 && tracks.Count == 1)
            {
                var window = Application.Current.MainWindow as MetroWindow;
                StringBuilder textBuilder = new StringBuilder();
                textBuilder.AppendLine("There is one track available");
                textBuilder.AppendLine();
                textBuilder.AppendLine("routes are used by navigation-devices");
                textBuilder.AppendLine("tracks are to register where you have been");
                textBuilder.AppendLine();
                textBuilder.AppendLine("Do you want to convert it to a route?");
                var dialogText = textBuilder.ToString();
                var result = await window.ShowMessageAsync("Convert track to route", dialogText, MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative)
                {
                    var converter = new TrackToRouteConverter();
                    _wayPoints = converter.ConvertTrackToRoute(tracks[0]);
                    // convert track to route
                    SetRoute();

                }
            }
            if(waypoints.Count > 0)
            {
                ShowWayPoints(waypoints);
            }

        }

        private void ShowWayPoints(List<wptType> waypoints)
        {
            var poiSymbol = this.Resources["POISymbol"] as SimpleMarkerSymbol;
            foreach (var point in waypoints)
            {
                var mapPoint = new Graphic(new MapPoint((double)point.lon, (double)point.lat, new SpatialReference(4326)), poiSymbol);
                mapPoint.Attributes["title"] = point.name;
                _poiLayer.Graphics.Add(mapPoint);
            }
        }

        private void SetRoute()
        {
            ZoomToRoute(_wayPoints);
            // bepaal afstand gehele route
            _distanceAnalyzer = new DistanceAnalyzer();
            length.Text = _distanceAnalyzer.CalculateDistance(_wayPoints).ToString();
            // teken begin en eind punt op de kaart
            GetStartAndEndPoint(grid.Resources["StartSymbol"] as SimpleMarkerSymbol, grid.Resources["EndSymbol"] as SimpleMarkerSymbol, _wayPoints);
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
            track.Attributes["routename"] = "mannus";
            _routelayer.Graphics.Clear();
            _routelayer.Graphics.Add(track);
            var startPoint = new Graphic(new MapPoint((double)wayPoints.First().lon, (double)wayPoints.First().lat, new SpatialReference(4326)), startSymbol);
            var endPoint = new Graphic(new MapPoint((double)wayPoints.Last().lon, (double)wayPoints.Last().lat, new SpatialReference(4326)), endSymbol);
            startPoint.Attributes["name"] = "start";
            endPoint.Attributes["name"] = "end";
            _routelayer.Graphics.Add(startPoint);
            _routelayer.Graphics.Add(endPoint);
        }

        private void DisplayPartOfTrack(List<wptType> wayPoints, Color color)
        {
            var lineSymbol = new SimpleLineSymbol();
            lineSymbol.Width = 4;
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
            var pois = GetPOIs();
            // TODO pois bufferen om route
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
            var routeSplitter = new RouteSplitter(_wayPoints);
            routeSplitter.SplitRoute(int.Parse(distanceTxt.Text));
            // collectie van routes en verschillende kleuren
            var x = routeSplitter.SplittedRoutes;
            _routeParts = x;
            var items = ColorUtils.RgbLinearInterpolate(Colors.LightBlue, Colors.DarkBlue, x.Count, Colors.LightBlue, Colors.DarkBlue);
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Map.Layers[0].IsVisible)
            {
                Map.Layers[0].IsVisible = false;
                Map.Layers[1].IsVisible = true;
                return;
            }
            if (Map.Layers[1].IsVisible)
            {
                Map.Layers[0].IsVisible = true;
                Map.Layers[1].IsVisible = false;
                return;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Map.Layers[2].IsVisible = !Map.Layers[2].IsVisible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _wayPoints.Reverse();
            SetRoute();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var pois = GetPOIs();
            var filename = _originalFile;
            var gpxFile = new GPXFile();
            var gpx = new gpxType();
            var rte = new rteType();
            rte.rtept = _wayPoints.ToArray();
            gpx.rte = new List<rteType>() { rte }.ToArray();
            gpx.wpt = pois;
            gpxFile.Save(filename, gpx);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/MannusEtten/BikeTouringGIS"))
                {
                    using (var result = await mgr)
                    {
                        var updateInfo = await result.CheckForUpdate();
                        var currentVersion = updateInfo.CurrentlyInstalledVersion.Version;
                        var futureVersion = updateInfo.FutureReleaseEntry.Version;
                        if (currentVersion != futureVersion)
                        {
                            var window = Application.Current.MainWindow as MetroWindow;
                            var controller = await window.ShowProgressAsync("Please wait...", "Updating application");
                            await result.UpdateApp();
                            await controller.CloseAsync();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ILogger logger = Logger.GetLogger();
                logger.LogException(ex);
            }
        }
    }
}
