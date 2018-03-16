using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using System.Collections.Generic;

namespace BikeTouringGIS.Comparers
{
    public class GraphicComparer : IEqualityComparer<Graphic>
    {
        /// <summary>
        /// vergelijkt twee graphics met elkaar, aanname is dat beide wel punten moeten zijn
        /// </summary>
        public bool Equals(Graphic x, Graphic y)
        {
            if (x.Geometry.GeometryType == GeometryType.Point && y.Geometry.GeometryType == GeometryType.Point)
            {
                var pX = x.Geometry as MapPoint;
                var pY = y.Geometry as MapPoint;
                return pX.X == pY.X && pX.Y == pY.Y && x.Attributes["filename"].Equals(y.Attributes["filename"]);
            }
            return false;
        }

        public int GetHashCode(Graphic obj)
        {
            int hCode = obj.Attributes["filename"].GetHashCode() ^ obj.Geometry.GetHashCode();
            return hCode.GetHashCode();
        }
    }
}