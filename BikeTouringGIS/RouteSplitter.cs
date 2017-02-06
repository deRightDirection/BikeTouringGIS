using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Symbology;
using GPX;
using Esri.ArcGISRuntime.Layers;
using BikeTouringGISLibrary;
using Esri.ArcGISRuntime.Geometry;
using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Model;

namespace BikeTouringGIS
{
    class RouteSplitter
    {
        private List<wptType> _wayPoints;
        private List<double> _distances;
        private List<List<wptType>> _splitRoutes = new List<List<wptType>>();
        private List<wptType> _splitPoints = new List<wptType>();
        private List<int> _splitIndices = new List<int>();
        private int _splitDistance;

        private void CreateSplittedRoutes()
        {
            var allIndices = _splitIndices.ToList();
            allIndices.Insert(0, 0);
            allIndices.Add(_wayPoints.Count - 1);
            for (int i = 0; i < allIndices.Count - 1;i++)
            {
                _splitRoutes.Add(_wayPoints.GetRange(allIndices[i], (allIndices[i+1] - allIndices[i]) + 1));
            }
        }

        private void CalculateSplitPoints()
        {
            _splitIndices.Clear();
            var length = 0.0;
            var splitCounter = 1;
            for (int i = 0; i < _wayPoints.Count - 1; i++)
            {
                length += _distances[i];
                var temporaryLength = _splitDistance * 1000 * splitCounter;
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

        internal void SplitRoute(int splitDistance, List<wptType> points)
        {
            _splitDistance = splitDistance;
            _wayPoints = points;
            CalculateOriginalDistances();
            CalculateSplitPoints();
            _splitPoints.Clear();
            _splitRoutes.Clear();
            _splitIndices.ForEach(x => _splitPoints.Add(_wayPoints[x]));
            CreateSplittedRoutes();
        }

        internal IEnumerable<BikeTouringGISGraphic> GetSplitPoints()
        {
            var result = new List<BikeTouringGISGraphic>();
            var sr = new SpatialReference(4326);
            var cumulativeDistance = 0;
            var startPoint = _wayPoints[0];
            for(int i =0; i < _splitPoints.Count; i++)
            {
                var point = _splitPoints[i];
                var mapPoint = new MapPoint((double)point.lon, (double)point.lat, sr);
                var graphic = new BikeTouringGISGraphic(mapPoint, GraphicType.SplitPoint);
                cumulativeDistance += GetDistance(startPoint, point);
                startPoint = point;
                graphic.Attributes["distance"] = cumulativeDistance;
                result.Add(graphic);
            }
            return result;
        }

        private int GetDistance(wptType startPoint, wptType point)
        {
            var startIndex = _wayPoints.IndexOf(startPoint);
            var lastIndex = _wayPoints.IndexOf(point);
            var distance = _distances.Skip(startIndex).Take(lastIndex - startIndex).Sum() / 1000;
            return (int)distance;
        }

        internal IEnumerable<BikeTouringGISGraphic> GetSplittedRoutes()
        {
            var result = new List<BikeTouringGISGraphic>();
            var sr = new SpatialReference(4326);
            foreach(var splitRoute in _splitRoutes)
            {
                var geometry = CreateGeometryFromWayPoints(splitRoute);
                var graphic = new BikeTouringGISGraphic(geometry, GraphicType.SplitRoute);
                result.Add(graphic);
            }
            return result;
        }

        private Polyline CreateGeometryFromWayPoints(List<wptType> wayPoints)
        {
            var builder = new PolylineBuilder(new SpatialReference(4326));
            foreach (var wayPoint in wayPoints)
            {
                builder.AddPoint(new MapPoint((double)wayPoint.lon, (double)wayPoint.lat));
            }
            return builder.ToGeometry();
        }

        public IEnumerable<IRoute> SplitRoutes
        {
            get
            {
                var result = new List<IRoute>();
                foreach(var routePoints in _splitRoutes)
                {
                    result.Add(new Route(routePoints));
                }
                return result;
            }
        }
    }
}