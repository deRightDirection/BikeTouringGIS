using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Geometry;
using GPX;

namespace BikeTouringGISLibrary.Model
{
    public class WayPoint : GeometryCreatable
    {
        public decimal Lat { get; private set; }
        public decimal Lon { get; private set; }
        public string Name { get; internal set; }
        public string Source { get; internal set; }

        public static implicit operator wptType(WayPoint waypoint)
        {
            var wptType = new wptType();
            wptType.name = waypoint.Name;
            wptType.lon = waypoint.Lon;
            wptType.lat = waypoint.Lat;
            return wptType;
        }

        public BikeTouringGISGraphic ToGraphic()
        {
            var graphic = CreateBikeTouringGISGraphic(Name, GraphicType.PointOfInterest);
            graphic.Attributes["source"] = Source;
            return graphic;
        }

        internal override void CreateGeometry()
        {
            Lat = Points[0].lat;
            Lon = Points[0].lon;
            Geometry = new MapPoint((double)Lon, (double)Lat, new SpatialReference(4326));
            Extent = Geometry.Extent;
        }
    }
}