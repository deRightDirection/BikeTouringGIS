using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Services;
using Esri.ArcGISRuntime.Geometry;
using GPX;
using System.Collections.Generic;
using WinUX;
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
            GeometryOperator.ReverseRoute(this);
        }
    }
}