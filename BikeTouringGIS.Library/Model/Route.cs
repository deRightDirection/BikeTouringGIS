using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Services;
using Esri.ArcGISRuntime.Geometry;

namespace BikeTouringGISLibrary.Model
{
    public class Route : Track
    {
        public Route()
        {
            Type = PathType.Route;
        }
        public void Flip()
        {
            Points.Reverse();
            var geometryFactory = new GeometryFactory();
            geometryFactory.CreateRouteGeometry(this);
        }
    }
}