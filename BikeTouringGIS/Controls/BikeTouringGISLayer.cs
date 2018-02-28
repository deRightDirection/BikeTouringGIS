using BikeTouringGIS.Services;
using BikeTouringGISLibrary;
using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Model;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using GPX;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace BikeTouringGIS.Controls
{
    public class BikeTouringGISLayer : GraphicsLayer
    {
        private IPath _routeOrTrack;
        private BikeTouringGISLayer _splitLayer;
        private Dictionary<GraphicType, object> _symbols;
        private bool _isInEditMode, _isSplitted, _isSelected;
        private string _splitPrefix, _title;
        private RouteSplitter _routeSplitter;
        private int _totalLength, _splitDistance;
        private Envelope _extent;

        private BikeTouringGISLayer()
        {
            Graphics.CollectionChanged += SetVisibility;
            _routeSplitter = new RouteSplitter();
        }

        public BikeTouringGISLayer(string name) : this()
        {
            FileName = name;
            DisplayName = name;
            Title = name;
            Type = LayerType.PointsOfInterest;
            SplitLayer = new BikeTouringGISLayer() { ShowLegend = false, Type = LayerType.SplitRoutes, SelectionColor = Colors.LimeGreen };
        }

        public BikeTouringGISLayer(string fileName, IPath routeOrTrack) : this(fileName)
        {
            _routeOrTrack = routeOrTrack;
            Title = string.IsNullOrEmpty(_routeOrTrack.Name) ? Path.GetFileNameWithoutExtension(fileName) : _routeOrTrack.Name;
            var subStringLength = Title.Length > 15 ? 15 : Title.Length;
            SplitPrefix = Title.Substring(0, subStringLength);
            Graphics.Add(_routeOrTrack.StartLocation);
            Graphics.Add(_routeOrTrack.EndLocation);
            Graphics.Add(_routeOrTrack.Geometry);
            SetLength();
            SelectionColor = Colors.LimeGreen;
            switch(routeOrTrack.Type)
            {
                case PathType.Route: Type = LayerType.GPXRoute;
                    break;
                case PathType.Track: Type = LayerType.GPXTrack;
                    break;
            }
            IsInEditMode = false;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    if (SplitLayer != null)
                    {
                        SplitLayer.IsSelected = value;
                        SplitLayer.SetSelectionColorOfGraphics();
                    }
                    SetSelectionColorOfGraphics();
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        private void SetSelectionColorOfGraphics()
        {
            foreach (BikeTouringGISGraphic graphic in Graphics)
            {
                if (graphic.Type == GraphicType.GPXRoute || graphic.Type == GraphicType.SplitRoute)
                {
                    graphic.IsSelected = IsSelected;
                }
            }
        }

        public string SplitPrefix
        {
            get { return _splitPrefix; }
            set
            {
                if (value != _splitPrefix)
                {
                    _splitPrefix = value;
                    OnPropertyChanged("SplitPrefix");
                }
            }
        }

        public bool IsSplitted
        {
            get { return _isSplitted; }
            set
            {
                if (value != _isSplitted)
                {
                    _isSplitted = value;
                    OnPropertyChanged("IsSplitted");
                }
            }
        }

        public BikeTouringGISLayer SplitLayer
        {
            get { return _splitLayer; }
            set
            {
                if (value != _splitLayer)
                {
                    _splitLayer = value;
                    OnPropertyChanged("SplitLayer");
                }
            }
        }

        internal void RemoveSplitRoutes()
        {
            IsSplitted = false;
            Opacity = SplitLayer.Opacity;
            SplitLayer.Graphics.Clear();
            IsVisible = true;
        }

        public bool IsInEditMode
        {
            get
            {
                if (Type == LayerType.PointsOfInterest)
                {
                    return false;
                }
                return _isInEditMode;
            }
            set
            {
                if (value != _isInEditMode)
                {
                    _isInEditMode = value;
                    OnPropertyChanged("IsInEditMode");
                }
            }
        }

        public int TotalLength
        {
            get { return _totalLength; }
            set
            {
                if (value != _totalLength)
                {
                    _totalLength = value;
                    OnPropertyChanged("TotalLength");
                }
            }
        }

        internal void Save(IEnumerable<wptType> waypoints, string fileName = null)
        {
            var gpxFile = new GPXFile();
            var gpx = new gpxType();
            var rte = new rteType();
            rte.name = Title;
            rte.rtept = ToRoute().Points.ToArray();
            gpx.rte = new List<rteType>() { rte }.ToArray();
            gpx.wpt = waypoints.ToArray();
            var fileNameToSave = string.IsNullOrEmpty(fileName) ? FileName : fileName;
            gpxFile.Save(fileNameToSave, gpx);
            IsInEditMode = false;
        }

        private void SetLength()
        {
            var length = 0;
            foreach (BikeTouringGISGraphic graphic in Graphics)
            {
                var distanceAnalyzer = new DistanceAnalyzer();
                var distance = distanceAnalyzer.CalculateDistance(graphic?.Geometry);
                length += distance;
            }
            TotalLength = length;
        }

        private void SetVisibility(object sender, NotifyCollectionChangedEventArgs e)
        {
            ShowLegend = Graphics.Count > 0;
        }

        public LayerType Type { get; private set; }

        internal void FlipDirection()
        {
            if (Type == LayerType.GPXRoute)
            {
                Graphics.Clear();
                var route = (Route)_routeOrTrack;
                RemoveRouteGeometries();
                route.Flip();
                Graphics.Add(_routeOrTrack.StartLocation);
                Graphics.Add(_routeOrTrack.EndLocation);
                Graphics.Add(_routeOrTrack.Geometry);
                SetSymbols();
                IsInEditMode = true;
                if (IsSplitted)
                {
                    SplitRoute(_splitDistance);
                }
            }
        }

        private void RemoveRouteGeometries()
        {
            Graphics.Remove(_routeOrTrack.StartLocation);
            Graphics.Remove(_routeOrTrack.EndLocation);
            Graphics.Remove(_routeOrTrack.Geometry);
        }

        internal void SetSymbolsAndSplitLayerDefaultProperties(Dictionary<GraphicType, object> symbols)
        {
            _symbols = symbols;
            SetSymbols();
            if (Type == LayerType.GPXRoute)
            {
                SplitLayer = new BikeTouringGISLayer() { Type = LayerType.SplitRoutes, IsVisible = false, ShowLegend = false, SelectionColor = Colors.LimeGreen };
                SplitLayer.Labeling.IsEnabled = true;
                SplitLayer.Labeling.LabelClasses.Add(new AttributeLabelClass() { Symbol = (TextSymbol)_symbols[GraphicType.SplitPointLabel], TextExpression = "[distance]" });
            }
            if (Type == LayerType.PointsOfInterest)
            {
                Labeling.IsEnabled = true;
                Labeling.LabelClasses.Add(new AttributeLabelClass() { Symbol = (TextSymbol)_symbols[GraphicType.PoILabelXL], TextExpression = "[name]", MinScale = 49999 });
                Labeling.LabelClasses.Add(new AttributeLabelClass() { Symbol = (TextSymbol)_symbols[GraphicType.PoILabelL], TextExpression = "[name]", MaxScale = 50000, MinScale = 99999 });
                Labeling.LabelClasses.Add(new AttributeLabelClass() { Symbol = (TextSymbol)_symbols[GraphicType.PoILabelM], TextExpression = "[name]", MaxScale = 100000 });
            }
        }

        private void SetSymbols(GraphicCollection graphics = null)
        {
            if (graphics == null)
            {
                graphics = Graphics;
            }
            foreach (BikeTouringGISGraphic graphic in graphics)
            {
                var symbol = _symbols[graphic.Type];
                if (symbol is Symbol)
                {
                    graphic.Symbol = (Symbol)_symbols[graphic.Type];
                }
            }
        }

        internal void SetExtentToFitWithWaypoints(Envelope wayPointsExtent)
        {
            if (wayPointsExtent != null)
            {
                Extent = Extent.Union(wayPointsExtent);
            }
        }

        public Envelope Extent
        {
            get
            {
                if (_extent == null)
                {
                    foreach (var graphic in Graphics)
                    {
                        var graphicExtent = graphic.Geometry.Extent;
                        _extent = _extent == null ? graphicExtent : _extent.Union(graphicExtent);
                    }
                }
                return _extent;
            }
            set
            {
                if (value != _extent)
                {
                    _extent = value;
                    OnPropertyChanged("Extent");
                }
            }
        }

        public string FileName { get; private set; }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value.Trim();
                    IsInEditMode = true;
                    OnPropertyChanged("Title");
                }
            }
        }

        internal void SplitRoute(int splitDistance)
        {
            _splitDistance = splitDistance;
            SplitLayer.DisplayName = $"{splitDistance} km";
            SplitLayer.Graphics.Clear();
            SplitLayer.Opacity = Opacity;
            _routeSplitter.SplitRoute(_splitDistance, _routeOrTrack.Points);
            var splitPoints = _routeSplitter.GetSplitPoints();
            var splitRoutes = _routeSplitter.GetSplittedRoutes();
            SplitLayer.Graphics.Add(_routeOrTrack.StartLocation);
            SplitLayer.Graphics.Add(_routeOrTrack.EndLocation);
            SplitLayer.Graphics.AddRange(splitPoints);
            SplitLayer.Graphics.AddRange(splitRoutes);
            SetSymbols(SplitLayer.Graphics);
            SplitLayer.IsVisible = true;
            IsVisible = false;
            IsSplitted = true;
            SplitLayer.SetSelectionColorOfGraphics();
        }

        public IEnumerable<IPath> SplitRoutes
        {
            get
            {
                return _routeSplitter.SplitRoutes;
            }
        }

        // later refactoren want dit moet dus in de speciale POILayer
        protected List<WayPoint> _wayPoints = new List<WayPoint>();

        public List<WayPoint> WayPoints
        {
            get
            {
                return _wayPoints;
            }
        }

        public IPath ToRoute()
        {
            return _routeOrTrack;
        }
    }
}