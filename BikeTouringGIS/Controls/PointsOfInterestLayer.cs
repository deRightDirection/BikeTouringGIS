using BikeTouringGIS.Comparers;
using BikeTouringGISLibrary.Model;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGIS.Controls
{
    public class PointsOfInterestLayer : BikeTouringGISLayer
    {
        public PointsOfInterestLayer(string name) : base(name)
        {
        }

        // ook refactoren en in aparte poilayer plaatsen
        internal void RemovePoIs(IEnumerable<WayPoint> wayPoints)
        {
            foreach (var wayPoint in WayPoints)
            {
                var g = wayPoint.ToGraphic();
                var index = FindIndexOfPointOfInterest(g);
                Graphics.RemoveAt(index);
            }
        }

        private int FindIndexOfPointOfInterest(Graphic g)
        {
            int index = -1;
            var comparer = new GraphicComparer();
            for(int i = 0; i < Graphics.Count; i++)
            {
                var graphicToCompare = Graphics[i];
                if(comparer.Equals(g, graphicToCompare))
                {
                    return i;
                }
            }
            return index;
        }
    }
}
