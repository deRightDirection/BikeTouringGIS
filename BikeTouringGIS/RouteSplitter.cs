using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPX;

namespace BikeTouringGIS
{
    class RouteSplitter
    {
        private List<wptType> _wayPoints;
        private List<double> _distances;
        private List<List<wptType>> _routes = new List<List<wptType>>();
        private List<wptType> _splitPoints = new List<wptType>();
        private List<int> _splitIndices = new List<int>();

        public RouteSplitter(List<wptType> route)
        {
            _wayPoints = route;
        }

        public void SplitRoute(int lengthToSplitInKilometers)
        {
            CalculateOriginalDistances();
            CalculateSplitPoints(lengthToSplitInKilometers);
            CreateSplitPoints();
            CreateSplittedRoutes();

        }

        private void CreateSplittedRoutes()
        {
            var allIndices = _splitIndices.ToList();
            allIndices.Insert(0, 0);
            allIndices.Add(_wayPoints.Count - 1);
            for (int i = 0; i < allIndices.Count - 1;i++)
            {
                _routes.Add(_wayPoints.GetRange(allIndices[i], (allIndices[i+1] - allIndices[i]) + 1));
            }
        }

        private void CreateSplitPoints()
        {
            foreach(var splitIndex in _splitIndices)
            {
                SplitPoints.Add(_wayPoints[splitIndex]);
            }
        }

        private void CalculateSplitPoints(int lengthToSplit)
        {
            _splitIndices.Clear();
            var length = 0.0;
            var splitCounter = 1;
            for (int i = 0; i < _wayPoints.Count - 1; i++)
            {
                length += _distances[i];
                var temporaryLength = lengthToSplit * 1000 * splitCounter;
                if (length > temporaryLength)
                {
                    var lengthAfter = length - temporaryLength;
                    var lengthBefore = temporaryLength - (length - _distances[i]);
                    if (lengthAfter > lengthBefore)
                    {
                        _splitIndices.Add(i);
                    }
                    else
                    {
                        _splitIndices.Add(i - 1);
                    }
                    splitCounter++;
                }
            }
        }

        private void CalculateOriginalDistances()
        {
            var dataAnalyzer = new DataAnalyzer(_wayPoints);
            _distances = dataAnalyzer.Distances;
        }

        public List<wptType> SplitPoints
        {
            get { return _splitPoints;  }
        }

        public List<List<wptType>> SplittedRoutes
        {
            get { return _routes; }
        }

    }
}