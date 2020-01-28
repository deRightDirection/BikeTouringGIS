using BikeTouringGIS.Controls;
using BikeTouringGIS.Extensions;
using BikeTouringGIS.Messenges;
using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.GPX;
using BikeTouringGISLibrary.Model;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using GalaSoft.MvvmLight.Command;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BikeTouringGIS.ViewModels
{
    public class BikeTouringGISMapViewModel : BikeTouringGISBaseViewModel
    {
        public RelayCommand SetupMapCommand { get; private set; }
        public RelayCommand<BikeTouringGISLayer> LayerLoadedCommand { get; private set; }
        public RelayCommand<BikeTouringGISLayer> RemoveLayerCommand { get; private set; }
        public RelayCommand<BikeTouringGISLayer> SaveLayerCommand { get; private set; }
        public RelayCommand<BikeTouringGISLayer> SaveLayerAsCommand { get; private set; }
        private PointsOfInterestLayer _pointsOfInterestLayer;
        private ObservableCollection<BikeTouringGISLayer> _bikeTouringGISLayers;
        private Dictionary<GraphicType, object> _mapSymbols;
        private bool _showKnooppunten, _showOpenCycleMap, _showOpenStreetMap, _mapSetupIsDone;
        private int _totalLengthOfRoutes;

        public BikeTouringGISMapViewModel()
        {
            SetupMapCommand = new RelayCommand(SetupMap);
            LayerLoadedCommand = new RelayCommand<BikeTouringGISLayer>(LayerLoaded);
            RemoveLayerCommand = new RelayCommand<BikeTouringGISLayer>(RemoveLayer);
            SaveLayerCommand = new RelayCommand<BikeTouringGISLayer>(SaveLayer);
            SaveLayerAsCommand = new RelayCommand<BikeTouringGISLayer>(SaveLayerAs);
            _mapSymbols = new Dictionary<GraphicType, object>();
            MessengerInstance.Register<ExtentChangedMessage>(this, SetNewExtent);
        }

        /// <summary>
        /// constructor alleen voor unit-testing, moet wel gerefactored worden
        /// https://github.com/MannusEtten/BikeTouringGIS/issues/83
        /// </summary>
        internal BikeTouringGISMapViewModel(bool createMap) : this()
        {
            _map = new Map();
            var poiLayer = new PointsOfInterestLayer("Points of Interest");
            _map.Layers.Add(poiLayer);
            _pointsOfInterestLayer = poiLayer;
            LayerLoaded(_pointsOfInterestLayer);
            MapView = new MapView() { Map = _map };
        }

        private void SaveLayerAs(BikeTouringGISLayer obj)
        {
            if (obj != null)
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "GPX files (*.gpx)|*.gpx";
                saveFileDialog.InitialDirectory = DropBoxHelper.GetDropBoxFolder();
                if (saveFileDialog.ShowDialog() == true)
                {
                    obj.Save(GetWayPoints(obj), saveFileDialog.FileName);
                }
            }
        }

        private void SaveLayer(BikeTouringGISLayer obj)
        {
            if (obj != null)
            {
                obj.Save(GetWayPoints(obj));
            }
        }

        private void SetNewExtent(ExtentChangedMessage obj)
        {
            switch (obj.ReasonToChangeExtent)
            {
                case ExtentChangedReason.CenterLayer:
                    MapView.SetView(obj.Extent.Expand(1.1));
                    break;

                case ExtentChangedReason.CenterMap:
                    SetExtent();
                    break;

                case ExtentChangedReason.ZoomIn:
                    MapView.SetView(MapView.Extent.Expand(0.75));
                    break;

                case ExtentChangedReason.ZoomOut:
                    MapView.SetView(MapView.Extent.Expand(1.25));
                    break;

                case ExtentChangedReason.StatusBarZoomInOrZoomOut:
                    // TODO blokkeren dat extent groter wordt dan bepaald zoom extent
                    try
                    {
                        MapView.SetView(MapView.Extent.Expand(obj.ZoomFactor));
                    }
                    // error komt als je flink bent uitgezoomd en nog verder wil uitzoomen
                    catch (NullReferenceException e) { }
                    break;
            }
        }

        private void RemoveLayer(BikeTouringGISLayer obj)
        {
            var splitLayer = obj.SplitLayer;
            _map.Layers.Remove(obj);
            _map.Layers.Remove(splitLayer);
            BikeTouringGISLayers.Remove(obj);
            RemovePointsOfInterestOfRemovedLayer(obj);
            SetExtent();
            CalculateTotalLength();
            PlacePointsOfInterestLayerOnTop();
            MessengerInstance.Send(new LayerRemovedMessage() { Layer = obj });
        }

        private void RemovePointsOfInterestOfRemovedLayer(BikeTouringGISLayer obj)
        {
            var source = obj.FileName;
            var layersWithSameSource = BikeTouringGISLayers.Any(x => x.FileName.Equals(source) && x.Type == LayerType.GPXRoute);
            if (!layersWithSameSource)
            {
                var wayPointsToRemove = _pointsOfInterestLayer.WayPoints.Where(x => x.Source.Equals(source)).ToList();
                _pointsOfInterestLayer.RemovePoIs(wayPointsToRemove);
            }
        }

        private void LayerLoaded(BikeTouringGISLayer layer)
        {
            if (layer != null)
            {
                BikeTouringGISLayers = new ObservableCollection<BikeTouringGISLayer>(_map.GetBikeTouringGISLayers());
            }
        }

        public MapView MapView { get; internal set; }

        public int TotalLengthOfRoutes
        {
            get { return _totalLengthOfRoutes; }
            set { Set(ref _totalLengthOfRoutes, value); }
        }

        public ObservableCollection<BikeTouringGISLayer> BikeTouringGISLayers
        {
            get { return _bikeTouringGISLayers; }
            set { Set(ref _bikeTouringGISLayers, value); }
        }

        private void SetupMap()
        {
            if (!_mapSetupIsDone)
            {
                ShowOpenCycleMap = true;
                ShowOpenStreetMap = false;
                ShowKnooppunten = false;
                _pointsOfInterestLayer = new PointsOfInterestLayer("Points of Interest");
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
                if (osm != null)
                {
                    osm.IsVisible = value;
                }
            }
        }

        internal void AddRoutes(List<BikeTouringGISLayer> routes)
        {
            routes.ForEach(x => x.SetSymbolsAndSplitLayerDefaultProperties(_mapSymbols));
            routes.ForEach(x => _map.Layers.Add(x));
            routes.ForEach(x => _map.Layers.Add(x.SplitLayer));
            SetExtent();
            CalculateTotalLength();
            PlacePointsOfInterestLayerOnTop();
            routes.ForEach(x => LayerLoaded(x));
        }

        internal void AddTracks(List<BikeTouringGISLayer> tracks)
        {
            /*
            _map.Layers.Add(layer);
            SetExtent();
            CalculateTotalLength();
            LayerLoaded(layer);
            */
        }

        private void PlacePointsOfInterestLayerOnTop()
        {
            var poiLayer = _map.GetBikeTouringGISLayers().FirstOrDefault(x => x.Type == LayerType.PointsOfInterest);
            _map.Layers.Remove(poiLayer);
            _map.Layers.Add(poiLayer);
        }

        //TODO MME 30012017 checken of in Quartz de binding wel goed werkt!
        public bool ShowOpenStreetMap
        {
            get { return _showOpenStreetMap; }
            set
            {
                Set(ref _showOpenStreetMap, value);
                var osm = _map.Layers?["openstreetmap"] as OpenStreetMapLayer;
                if (osm != null)
                {
                    osm.IsVisible = value;
                }
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
                if (wms != null)
                {
                    wms.IsVisible = value;
                }
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

        /// <summary>
        /// zet de extent naar de union van extents van de lagen aanwezig in de kaart
        /// </summary>
        private void SetExtent()
        {
            Envelope initialExtent = null;
            foreach (var layer in _map.GetBikeTouringGISLayers())
            {
                initialExtent = initialExtent == null ? initialExtent = layer.Extent : initialExtent = initialExtent.Union(layer.Extent);
            }
            if (initialExtent != null)
            {
                MapView.SetView(initialExtent.Expand(1.2));
            }
        }

        internal void AddPoIs(List<WayPoint> wayPoints)
        {
            _pointsOfInterestLayer.AddPoIs(wayPoints);
            _pointsOfInterestLayer.SetSymbolsAndSplitLayerDefaultProperties(_mapSymbols);
        }

        private IEnumerable<wptType> GetWayPoints(BikeTouringGISLayer layer)
        {
            var source = layer.FileName;
            var selection = _pointsOfInterestLayer.WayPoints.Where(x => x.Source.Equals(source));
            var result = new List<wptType>();
            selection.ForEach(x => result.Add(x));
            return result;
        }
    }
}