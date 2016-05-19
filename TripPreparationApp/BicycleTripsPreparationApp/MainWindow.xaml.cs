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

namespace BicycleTripsPreparationApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "GPX files (*.gpx)|*.gpx";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = DropBoxHelper.GetDropBoxFolder();
            if (openFileDialog.ShowDialog() == true)
            {
                var trackReader = new TrackReader(openFileDialog.FileName);
                trackReader.ReadGPXTrack();
                GetTrackAsEsriRuntimeGeometry(grid.Resources["DashedGreenLineSymbol"] as SimpleLineSymbol, trackReader.WayPoints);
                GetStartAndEndPoint(grid.Resources["StartSymbol"] as SimpleMarkerSymbol, grid.Resources["EndSymbol"] as SimpleMarkerSymbol, trackReader.WayPoints);
                var distanceAnalyzer = new DistanceAnalyzer(trackReader.WayPoints);

                var layer = Map.Layers["graphics"] as GraphicsLayer;
                foreach(var point in trackReader.WayPoints)
                {
                    layer.Graphics.Add(new Graphic(new MapPoint((double)point.lon, (double)point.lat, new SpatialReference(4326)), grid.Resources["RouteSymbol"] as SimpleMarkerSymbol));
                }


                length.Text = distanceAnalyzer.TotalDistance;
            }
        }

        private void GetStartAndEndPoint(SimpleMarkerSymbol startSymbol, SimpleMarkerSymbol endSymbol, List<wptType> wayPoints)
        {
            var layer = Map.Layers["graphics"] as GraphicsLayer;
            layer.Graphics.Add(new Graphic(new MapPoint((double)wayPoints.First().lon, (double)wayPoints.First().lat, new SpatialReference(4326)), startSymbol));
            layer.Graphics.Add(new Graphic(new MapPoint((double)wayPoints.Last().lon, (double)wayPoints.Last().lat, new SpatialReference(4326)), endSymbol));
        }

        private void GetTrackAsEsriRuntimeGeometry(SimpleLineSymbol simpleLineSymbol, List<wptType> wayPoints)
        {
            var builder = new PolylineBuilder(new SpatialReference(4326));
            foreach (var wayPoint in wayPoints)
            {
                builder.AddPoint(new MapPoint((double)wayPoint.lon, (double)wayPoint.lat));
            }
            var layer = Map.Layers["graphics"] as GraphicsLayer;
            var track = new Graphic(builder.ToGeometry(), simpleLineSymbol);
            layer.Graphics.Add(track);
            MyMapView.SetView(track.Geometry.Extent.Expand(1.2));
        }
    }
}
