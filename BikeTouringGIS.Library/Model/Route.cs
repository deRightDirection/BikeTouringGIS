using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Services;
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
            Geometry = GeometryOperator.ReversePoints(Points);
        }
    }
}