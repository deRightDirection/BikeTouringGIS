using BikeTouringGISLibrary.GPX;
using Esri.ArcGISRuntime.Geometry;
using System.Collections.Generic;

namespace BikeTouringGISLibrary.Extensions
{
    public static class GeometryExtension
    {
        public static List<wptType> GetWayPoints(this Geometry geometry)
        {
            var wayPoints = new List<wptType>();
            if (geometry.GeometryType == GeometryType.Polyline)
            {
                var line = geometry as Polyline;
                foreach (var part in line.Parts)
                {
                    foreach (var point in part.GetPoints())
                    {
                        var wpt = new wptType();
                        wpt.lat = (decimal)point.Y;
                        wpt.lon = (decimal)point.X;
                        wayPoints.Add(wpt);
                    }
                }
            }
            return wayPoints;
        }
    }
}