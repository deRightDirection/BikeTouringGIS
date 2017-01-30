using BicycleTripsPreparationApp;
using BikeTouringGISLibrary.Model;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BikeTouringGIS.ViewModels
{
    public class MainScreenViewModel : ViewModelBase
    {
        private BikeTouringGISProject _project;

        private bool _showOpenCycleMap, _showOpenStreetMap, _showFietsknooppunten;
        private bool _poiClickMode;
        private readonly IDialogCoordinator _dialogCoordinator;
        private string _poiName;

        public RelayCommand ClearPOIsCommand { get; private set; }
        public RelayCommand LoadGpxFileCommand { get; private set; }
        public RelayCommand<MapPoint> MapDoubleClickCommand { get; private set; }
        public RelayCommand<int> SwitchBaseMapCommand { get; private set; }
        public RelayCommand SaveSplittedRoute { get; private set; }
        public RelayCommand AddPOICommand { get; private set; }
        public RelayCommand FlipDirectionCommand { get; private set; }

        public MainScreenViewModel()
        {
            _dialogCoordinator = DialogCoordinator.Instance;
            ShowFietsknooppunten = false;
            ShowOpenCycleMap = true;
            ShowOpenStreetMap = false;
            SwitchBaseMapCommand = new RelayCommand<int>(x => SwitchBaseMap(x));
            AddPOICommand = new RelayCommand(AddPOI);
            FlipDirectionCommand = new RelayCommand(FlipDirection);
            MapDoubleClickCommand = new RelayCommand<MapPoint>(x => MapDoubleClick(x));
            ClearPOIsCommand = new RelayCommand(ClearPOIs);
        }

        public BikeTouringGISProject Project
        {
            get { return _project; }
            set { Set(() => Project, ref _project, value); }
        }


        // before 1.1.0 code

        private void FlipDirection()
        {
        }

        // TODO: nog niet volledig mvvm
        private void ClearPOIs()
        {
            var window = App.Current.MainWindow;
            var mainScreen = ((MetroContentControl)window.Content).Content as MainScreen;
            var symbol = mainScreen.Resources["POISymbol"] as SimpleMarkerSymbol;
            var mapview = mainScreen.FindName("MyMapView") as MapView;
            var poiLayer = mapview.Map.Layers["POIs"] as GraphicsLayer;
            poiLayer.Graphics.Clear();
        }

        // TODO: nog niet volledig mvvm
        private void MapDoubleClick(MapPoint p)
        {
            if (_poiClickMode)
            {
                var window = App.Current.MainWindow;
                var mainScreen = ((MetroContentControl)window.Content).Content as MainScreen;
                var symbol = mainScreen.Resources["POISymbol"] as SimpleMarkerSymbol;
                var mapview = mainScreen.FindName("MyMapView") as MapView;
                var poiLayer = mapview.Map.Layers["POIs"] as GraphicsLayer;
                var poi = new Graphic(p, symbol);
                poi.Attributes["title"] = _poiName;
                poiLayer.Graphics.Add(poi);
                _poiClickMode = false;
            }
        }

        // TODO: nog niet volledig mvvm
        private async void AddPOI()
        {

            await _dialogCoordinator.ShowInputAsync(this, "Name of POI", "Fill in the name of POI and click ok.")
                .ContinueWith(t => _poiName = t.Result);
            if(string.IsNullOrEmpty(_poiName))
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Name of POI", "No name for POI given");
            }
            else
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Name of POI", "After closing this window double click on the map for the location of the POI");
                _poiClickMode = true;

            }
        }

        public bool ShowOpenCycleMap
        {
            get { return _showOpenCycleMap; }
            set { Set(() => ShowOpenCycleMap, ref _showOpenCycleMap, value); }
        }
        public bool ShowOpenStreetMap
        {
            get { return _showOpenStreetMap; }
            set { Set(() => ShowOpenStreetMap, ref _showOpenStreetMap, value); }
        }
        public bool ShowFietsknooppunten
        {
            get { return _showFietsknooppunten; }
            set { Set(() => ShowFietsknooppunten, ref _showFietsknooppunten, value); }
        }

        private void SwitchBaseMap(int baseMapParameter)
        {
            switch(baseMapParameter)
            {
                case 0: ShowOpenCycleMap = true;
                    ShowOpenStreetMap = false;
                    break;
                case 1:ShowOpenCycleMap = false;
                    ShowOpenStreetMap = true;
                    break;
                case 2:
                    ShowFietsknooppunten = !ShowFietsknooppunten;
                    break;
            }
        }
    }
}