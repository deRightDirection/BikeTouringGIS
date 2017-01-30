using BikeTouringGIS.Controls;
using BikeTouringGIS.Messenges;
using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUX.Common;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using BikeTouringGIS.Extensions;
using WinUX;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Windows;
using BikeTouringGISLibrary;

namespace BikeTouringGIS.ViewModels
{
    public class BikeTouringGISMapViewModel : BikeTouringGISBaseViewModel
    {
        public RelayCommand OpenGPXFileCommand { get; private set; }
        public RelayCommand SetupMapCommand { get; private set; }
        private BikeTouringGISLayer _pointsOfInterestLayer, _selectedLayer;
        private Dictionary<GraphicType, object> _mapSymbols;
        private bool _showKnooppunten, _showOpenCycleMap, _showOpenStreetMap, _mapSetupIsDone;
        private int _totalLengthOfRoutes;
        public BikeTouringGISMapViewModel()
        {
            OpenGPXFileCommand = new RelayCommand(OpenGPXFile);
            SetupMapCommand = new RelayCommand(SetupMap);
            _mapSymbols = new Dictionary<GraphicType, object>();
        }

        public MapView MapView { get; internal set; }
        public int TotalLengthOfRoutes
        {
            get { return _totalLengthOfRoutes; }
            set { Set(ref _totalLengthOfRoutes, value); }
        }

        private void SetupMap()
        {
            if (!_mapSetupIsDone)
            {
                ShowOpenCycleMap = false;
                ShowOpenStreetMap = true;
                ShowKnooppunten = false;
                _pointsOfInterestLayer = new BikeTouringGISLayer("Points of Interest");
                _map.Layers.Add(_pointsOfInterestLayer);
                _map.Layers.ForEach(x => x.ShowLegend = false);
                _mapSetupIsDone = true;
            }
        }

        //TODO MME 30012017 checken of in Quartz de binding wel goed werkt!
        public bool ShowOpenCycleMap
        {
            get { return _showOpenCycleMap; }
            set
            {
                Set(ref _showOpenCycleMap, value);
                var osm = _map.Layers?["opencyclemap"] as OpenStreetMapLayer;
                osm.IsVisible = value;
            }
        }
        //TODO MME 30012017 checken of in Quartz de binding wel goed werkt!
        public bool ShowOpenStreetMap
        {
            get { return _showOpenStreetMap; }
            set
            {
                Set(ref _showOpenStreetMap, value);
                var osm = _map.Layers?["openstreetmap"] as OpenStreetMapLayer;
                osm.IsVisible = value;
            }
        }
        //TODO MME 30012017 checken of in Quartz de binding wel goed werkt!
        public bool ShowKnooppunten
        {
            get { return _showKnooppunten; }
            set
            {
                Set(ref _showKnooppunten, value);
                var wms = _map.Layers?["fietsknooppunten"] as WmsLayer;
                wms.IsVisible = value;
            }
        }

        internal void AddSymbol(GraphicType typeOfSymbol, object symbol)
        {
            _mapSymbols.Add(typeOfSymbol, symbol);
        }

        private void CalculateTotalLength()
        {
            var length = 0;
            _map.GetBikeTouringGISLayers().ForEach(x => length += x.TotalLength);
            TotalLengthOfRoutes = length;
        }

        private void SetExtent()
        {
            Envelope initialExtent = null;
            foreach (var layer in _map.GetBikeTouringGISLayers())
            {
                    initialExtent = initialExtent == null ? initialExtent = layer.Extent : initialExtent = initialExtent.Union(layer.Extent);
            }
            if(initialExtent != null)
            {
                MapView.SetView(initialExtent.Expand(1.2));
            }
        }

        private async void OpenGPXFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "GPX files (*.gpx)|*.gpx";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = DropBoxHelper.GetDropBoxFolder();
            if (openFileDialog.ShowDialog() == true)
            {
                var gpxFileInformation = new GpxFileReader().LoadFile(openFileDialog.FileName);
                foreach (var track in gpxFileInformation.Tracks)
                {
                    StringBuilder textBuilder = new StringBuilder();
                    textBuilder.AppendLine($"Track {track.Name} is defined as track and not as route");
                    textBuilder.AppendLine();
                    textBuilder.AppendLine("routes are used by navigation-devices");
                    textBuilder.AppendLine("tracks are to register where you have been");
                    textBuilder.AppendLine();
                    textBuilder.AppendLine("Do you want to convert it to a route?");
                    var convertTrack = await ConvertTrackToRoute(textBuilder.ToString());
                    if (convertTrack)
                    {
                        track.ConvertTrackToRoute();
                    }
                }
                gpxFileInformation.CreateGeometries();
                var layer = new BikeTouringGISLayer(openFileDialog.FileName, gpxFileInformation.AllRoutes);
                layer.SetSymbols(_mapSymbols);
                _map.Layers.Add(layer);
                SetExtent();
                CalculateTotalLength();
            }
        }

        private async Task<bool> ConvertTrackToRoute(string text)
        {
            var window = Application.Current.MainWindow as MetroWindow;
            var result = await window.ShowMessageAsync("Convert track to route", text, MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                return true;
            }
            return false;
        }
    }
}