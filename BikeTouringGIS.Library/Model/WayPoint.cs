using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using BikeTouringGISLibrary.Enumerations;
using GPX;

namespace BikeTouringGISLibrary.Model
{
    public class WayPoint : GeometryCreatable
    {
        public string Name { get; internal set; }
        public string Source { get; internal set; }
        public decimal Lat { get; private set; }
        public decimal Lon { get; private set; }
        internal override void CreateGeometry()
        {
            Lat = Points[0].lat;
            Lon = Points[0].lon;
            Geometry = new MapPoint((double)Lon, (double)Lat, new SpatialReference(4326));
            Extent = Geometry.Extent;
        }

        public BikeTouringGISGraphic ToGraphic()
        {
            var graphic = CreateBikeTouringGISGraphic(Name, GraphicType.PointOfInterest);
            graphic.Attributes["source"] = Source;
            return graphic;
        }

        public static implicit operator wptType(WayPoint waypoint)
        {
            var wptType = new wptType();
            wptType.name = waypoint.Name;
            wptType.lon = waypoint.Lon;
            wptType.lat = waypoint.Lat;
            return wptType;
        }
    }
}