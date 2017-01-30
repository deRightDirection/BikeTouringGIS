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

namespace BikeTouringGIS.Controls
{
    public class BikeTouringGISLayer : GraphicsLayer
    {
        public BikeTouringGISLayer(string name)
        {
            Graphics.CollectionChanged += SetVisibility;
            DisplayName = name;
            Type = LayerType.PointsOfInterest;
        }

        private void SetVisibility(object sender, NotifyCollectionChangedEventArgs e)
        {
           ShowLegend = Graphics.Count > 0;
        }

        public LayerType Type { get;private set;}

        public BikeTouringGISLayer(string fileName, IEnumerable<IRoute> routes) : this(fileName)
        {
            foreach(var route in routes)
            {
                Graphics.Add(route.StartLocation);
                Graphics.Add(route.EndLocation);
                Graphics.Add(route.RouteGeometry);
            }
            Type = LayerType.GPXRoutes;
        }

        public void SetSymbols(Dictionary<GraphicType, object> symbols)
        {
            foreach(BikeTouringGISGraphic graphic in Graphics)
            {
                var symbol = symbols[graphic.Type];
                if(symbol is Symbol)
                {
                    graphic.Symbol = (Symbol)symbols[graphic.Type];
                }
            }
        }

        public Envelope Extent
        {
            get
            {
                Envelope initialExtent = null;
                foreach (var graphic in Graphics)
                {
                    var graphicExtent = graphic.Geometry.Extent;
                    initialExtent = initialExtent == null ? initialExtent = graphicExtent : initialExtent = initialExtent.Union(graphicExtent);
                }
                return initialExtent;
            }
        }

        public int TotalLength
        {
            get
            {
                var length = 0;
                foreach (BikeTouringGISGraphic graphic in Graphics)
                {
                    var distanceAnalyzer = new DistanceAnalyzer();
                    var distance = distanceAnalyzer.CalculateDistance(graphic.Geometry);
                    length += distance;
                }
                return length;
            }
        }
    }
}