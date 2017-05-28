using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Geometry;
using GPX;
using System.Collections.Generic;

namespace BikeTouringGISLibrary
{
    public abstract class GeometryCreatable
    {
        public Envelope Extent { get; protected set; }
        public List<wptType> Points { get; internal set; }
        protected Geometry Geometry { get; set; }

        internal abstract void CreateGeometry();

        protected BikeTouringGISGraphic CreateBikeTouringGISGraphic(string nameAttribute, GraphicType typeOfGraphic)
        {
            var g = new BikeTouringGISGraphic(Geometry, typeOfGraphic);
            g.Attributes["name"] = nameAttribute;
            return g;
        }

        protected BikeTouringGISGraphic CreateBikeTouringGISPointGraphic(wptType location, string nameAttribute, GraphicType typeOfPoint)
        {
            var mappoint = new MapPoint((double)location.lon, (double)location.lat, new SpatialReference(4326));
            var g = new BikeTouringGISGraphic(mappoint, typeOfPoint);
            g.ZIndex = 1;
            g.Attributes["name"] = nameAttribute;
            return g;
        }
    }
}