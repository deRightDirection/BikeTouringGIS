using BikeTouringGISLibrary.Extensions;
using BikeTouringGISLibrary.GPX;
using Esri.ArcGISRuntime.Geometry;
using System.Linq;

namespace BikeTouringGISLibrary
{
    public class DistanceAnalyzer
    {
        public int CalculateDistance(Geometry routeGeometry)
        {
            if (routeGeometry == null || routeGeometry.GeometryType != GeometryType.Polyline)
            {
                return 0;
            }
            var wayPoints = routeGeometry.GetWayPoints();
            var dataAnalyzer = new DataAnalyzer(wayPoints);
            var distances = dataAnalyzer.Distances;
            return ((int)distances.Sum() / 1000);
        }
    }
}