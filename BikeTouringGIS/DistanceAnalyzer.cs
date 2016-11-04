using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPX;

namespace BikeTouringGIS
{
    class DistanceAnalyzer
    {
        public int CalculateDistance(List<wptType> wayPoints)
        {
            var dataAnalyzer = new DataAnalyzer(wayPoints);
            var distances = dataAnalyzer.Distances;
            return ((int)distances.Sum() / 1000);
        }
    }
}