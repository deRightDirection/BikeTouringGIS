using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using GPX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISLibrary
{
    public abstract class GeometryCreatable
    {
        protected Geometry Geometry { get; set; }
        public Envelope Extent { get; protected set; }
        public List<wptType> Points { get; internal set; }
        internal abstract void CreateGeometry();
        protected BikeTouringGISGraphic CreatePointGraphic(wptType point, string nameAttribute)
        {
            var g = new BikeTouringGISGraphic(new MapPoint((double)point.lon, (double)point.lat, new SpatialReference(4326)));
            g.Attributes["name"] = nameAttribute;
            return g;
        }
        protected BikeTouringGISGraphic CreateLineGraphic(string nameAttribute)
        {
            var g = new BikeTouringGISGraphic(Geometry);
            g.Attributes["name"] = nameAttribute;
            return g;
        }
    }
}