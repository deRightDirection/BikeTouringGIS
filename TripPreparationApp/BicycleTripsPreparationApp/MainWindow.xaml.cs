using Esri.ArcGISRuntime.Controls;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Geometry;
using GPX;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Forms;
using System.IO;

namespace BicycleTripsPreparationApp
{
    public partial class MainWindow : Window
    {
        private List<List<wptType>> _routeParts;
        private string _lastUsedFolder;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "GPX files (*.gpx)|*.gpx";
            var layer = Map.Layers["graphics"] as GraphicsLayer;
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = DropBoxHelper.GetDropBoxFolder();
            if (openFileDialog.ShowDialog() == true)
            {
                _lastUsedFolder = Path.GetDirectoryName(openFileDialog.FileName);
                layer.Graphics.Clear();
                var trackReader = new TrackReader(openFileDialog.FileName);
                // lees gpx tracks en zoom in naar gehele route
                trackReader.ReadGPXTrack();
                ZoomToRoute(trackReader.WayPoints);
                // bepaal afstand gehele route
                var distanceAnalyzer = new DistanceAnalyzer();
                length.Text = distanceAnalyzer.CalculateDistance(trackReader.WayPoints).ToString();
                // splits de route op naar stukken van x km
                var routeSplitter = new RouteSplitter(trackReader.WayPoints);
                routeSplitter.SplitRoute(100);
                // collectie van routes en verschillende kleuren
                var x = routeSplitter.SplittedRoutes;
                _routeParts = x;
                var items = ColorUtils.RgbLinearInterpolate(Colors.Green, Colors.Orange, Colors.Red, x.Count);
                // doorloop alle routes, teken iedere route op de kaart zet voor ieder beginpunt een stuk op de kaart plus afstand tot dit punt
                int distance = 0;
                for (int j = 0; j < x.Count;j++)
                {
                    DisplayPartOfTrack(x[j], items[j]);
                    if (j > 0)
                    {
                        var point = x[j][0];
                        var g = new Graphic(new MapPoint((double)point.lon, (double)point.lat, new SpatialReference(4326)), grid.Resources["RouteSymbol"] as SimpleMarkerSymbol);
                        g.Attributes["index"] = distance;
                        layer.Graphics.Add(g);
                    }
                    distance += distanceAnalyzer.CalculateDistance(x[j]);
                }
                // teken begin en eind punt op de kaart
                GetStartAndEndPoint(grid.Resources["StartSymbol"] as SimpleMarkerSymbol, grid.Resources["EndSymbol"] as SimpleMarkerSymbol, trackReader.WayPoints);
            }
        }

        private void ZoomToRoute(List<wptType> wayPoints)
        {
            var geometry = CreateGeometryFromWayPoints(wayPoints);
            MyMapView.SetView(geometry.Extent.Expand(1.2));
        }

        private void GetStartAndEndPoint(SimpleMarkerSymbol startSymbol, SimpleMarkerSymbol endSymbol, List<wptType> wayPoints)
        {
            var layer = Map.Layers["graphics"] as GraphicsLayer;
            layer.Graphics.Add(new Graphic(new MapPoint((double)wayPoints.First().lon, (double)wayPoints.First().lat, new SpatialReference(4326)), startSymbol));
            layer.Graphics.Add(new Graphic(new MapPoint((double)wayPoints.Last().lon, (double)wayPoints.Last().lat, new SpatialReference(4326)), endSymbol));
        }

        private void DisplayPartOfTrack(List<wptType> wayPoints, Color color)
        {
            var lineSymbol = new SimpleLineSymbol();
            lineSymbol.Width = 3;
            lineSymbol.Color = color;
            var layer = Map.Layers["graphics"] as GraphicsLayer;
            var geometry = CreateGeometryFromWayPoints(wayPoints);
            var track = new Graphic(geometry, lineSymbol);
            layer.Graphics.Add(track);
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
                for(int i=0; i < _routeParts.Count; i++)
                {
                    var filename = string.Format(@"{0}\{1}_{2}.gpx", _lastUsedFolder, prefix.Text, i + 1);
                    var gpxFile = new GPXFile();
                    var gpx = new gpxType();
                    var rte = new rteType();
                    rte.rtept = _routeParts[i].ToArray();
                    gpx.rte = new List<rteType>() { rte }.ToArray();
                    gpxFile.Save(filename, gpx);
                }
        }
    }
}
