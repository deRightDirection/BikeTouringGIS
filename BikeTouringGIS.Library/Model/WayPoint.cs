using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Geometry;
using GPX;

namespace BikeTouringGISLibrary.Model
{
    public class WayPoint : GeometryData
    {
        public decimal Lat { get; private set; }
        public decimal Lon { get; private set; }

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