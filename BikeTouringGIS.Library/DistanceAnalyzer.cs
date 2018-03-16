using BikeTouringGISLibrary.Extensions;
using Esri.ArcGISRuntime.Geometry;
using GPX;
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

        public void DoSomethingWithTheTrack(Geometry trackGeometry)
        {
            if (trackGeometry == null || trackGeometry.GeometryType != GeometryType.Polyline)
            {
//                return 0;
            }
            var wayPoints = trackGeometry.GetWayPoints();
            var dataAnalyzer = new DataAnalyzer(wayPoints);
//            dataAnalyzer.
            var distances = dataAnalyzer.Distances;
//            return ((int)distances.Sum() / 1000);

        }
    }
}