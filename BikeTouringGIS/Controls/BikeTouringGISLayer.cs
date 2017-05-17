using Esri.ArcGISRuntime.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeTouringGISLibrary.Model;
using BikeTouringGISLibrary;
using BikeTouringGISLibrary.Enumerations;
using System.Collections.Specialized;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Geometry;
using MoreLinq;
using System.Windows.Media;
using GPX;
using System.IO;

namespace BikeTouringGIS.Controls
{
    public class BikeTouringGISLayer : GraphicsLayer
    {
        private IRoute _route;
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
        public BikeTouringGISLayer(string fileName, IRoute route) : this(fileName)
        {
            _route = route;
            Title = string.IsNullOrEmpty(route.Name) ? Path.GetFileNameWithoutExtension(fileName) : route.Name;
            var subStringLength = Title.Length > 15 ? 15 : Title.Length;
            SplitPrefix = Title.Substring(0, subStringLength);
            Graphics.Add(route.StartLocation);
            Graphics.Add(route.EndLocation);
            Graphics.Add(route.RouteGeometry);
            SetLength();
            SelectionColor = Colors.LimeGreen;
            Type = LayerType.GPXRoute;
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
                if(value != _isSplitted)
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
                if(value != _splitLayer)
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
                if(Type == LayerType.PointsOfInterest)
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
                if(value != _totalLength)
                {
                    _totalLength = value;
                    OnPropertyChanged("TotalLength");
                }
            }
        }

        internal void Save(string fileName = null)
        {
            var gpxFile = new GPXFile();
            var gpx = new gpxType();
            var rte = new rteType();
            rte.name = Title;
            rte.rtept = ToRoute().Points.ToArray();
            gpx.rte = new List<rteType>() { rte }.ToArray();
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

        public LayerType Type { get;private set;}

        internal void FlipDirection()
        {
            Graphics.Clear();
            _route.Flip();
            Graphics.Add(_route.StartLocation);
            Graphics.Add(_route.EndLocation);
            Graphics.Add(_route.RouteGeometry);
            SetSymbols();
            IsInEditMode = true;
            if(IsSplitted)
            {
                SplitRoute(_splitDistance);
            }
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
            if(graphics == null)
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
                if(_extent == null)
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
                if(value != _extent)
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
            _routeSplitter.SplitRoute(_splitDistance, _route.Points);
            var splitPoints = _routeSplitter.GetSplitPoints();
            var splitRoutes = _routeSplitter.GetSplittedRoutes();
            SplitLayer.Graphics.Add(_route.StartLocation);
            SplitLayer.Graphics.Add(_route.EndLocation);
            SplitLayer.Graphics.AddRange(splitPoints);
            SplitLayer.Graphics.AddRange(splitRoutes);
            SetSymbols(SplitLayer.Graphics);
            SplitLayer.IsVisible = true;
            IsVisible = false;
            IsSplitted = true;
            SplitLayer.SetSelectionColorOfGraphics();
        }
        
        public IEnumerable<IRoute> SplitRoutes
        {
            get
            {
                return _routeSplitter.SplitRoutes;
            }
        }

        public IRoute ToRoute()
        {
            return _route;
        }
    }
}