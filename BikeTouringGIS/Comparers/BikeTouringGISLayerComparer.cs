using BikeTouringGIS.Controls;
using BikeTouringGISLibrary.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGIS.Comparers
{
    public class BikeTouringGISLayerComparer : IComparer<LayerType>
    {
        public int Compare(LayerType x, LayerType y)
        {
            return x.CompareTo(y);
//            if (x == LayerType.PointsOfInterest)
//            {
//                return -1;
//            }
//            if(x  != LayerType.PointsOfInterest && y == LayerType.PointsOfInterest)
//            {
 //               return 1;
  //          }
  //          return 0;
        }
    }
}
