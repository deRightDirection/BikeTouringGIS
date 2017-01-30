using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPX;
using Esri.ArcGISRuntime.Geometry;
using BikeTouringGISLibrary.Extensions;

namespace BikeTouringGISLibrary
{
    public class DistanceAnalyzer
    {
        public int CalculateDistance(Geometry routeGeometry)
        {
            if(routeGeometry.GeometryType != GeometryType.Polyline)
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