using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPX;

namespace BicycleTripsPreparationApp
{
    class DistanceAnalyzer
    {
        private List<wptType> _wayPoints;
        private List<double> _distances;
        public DistanceAnalyzer(List<wptType> wayPoints)
        {
            _wayPoints = wayPoints;
            var dataAnalyzer = new DataAnalyzer(_wayPoints);
            _distances = dataAnalyzer.Distances;
        }

        public string TotalDistance
        {
            get
            {
                return ((int)_distances.Sum() / 1000).ToString();
            }
        }
    }
}