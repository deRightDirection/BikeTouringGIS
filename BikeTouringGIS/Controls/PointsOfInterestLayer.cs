using BikeTouringGIS.Comparers;
using BikeTouringGISLibrary.Model;
using Esri.ArcGISRuntime.Layers;
using System.Collections.Generic;
using theRightDirection.Library;

namespace BikeTouringGIS.Controls
{
    public class PointsOfInterestLayer : BikeTouringGISLayer
    {
        public PointsOfInterestLayer(string name) : base(name)
        {
        }

        internal void AddPoIs(IEnumerable<WayPoint> wayPoints)
        {
            _wayPoints.AddRange(wayPoints);
            wayPoints.ForEach(wp => Graphics.Add(wp.Geometry));
        }

        // ook refactoren en in aparte poilayer plaatsen
        internal void RemovePoIs(IEnumerable<WayPoint> wayPoints)
        {
            foreach (var wayPoint in wayPoints)
            {
                var g = wayPoint.Geometry;
                var index = FindIndexOfPointOfInterest(g);
                if (index > -1)
                {
                    _wayPoints.Remove(wayPoint);
                    Graphics.RemoveAt(index);
                }
            }
        }

        private int FindIndexOfPointOfInterest(Graphic g)
        {
            int index = -1;
            var comparer = new GraphicComparer();
            for (int i = 0; i < Graphics.Count; i++)
            {
                var graphicToCompare = Graphics[i];
                if (comparer.Equals(g, graphicToCompare))
                {
                    return i;
                }
            }
            return index;
        }
    }
}