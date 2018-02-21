using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Services;
using Esri.ArcGISRuntime.Geometry;
using GPX;
using System.Collections.Generic;

namespace BikeTouringGISLibrary.Model
{
    public class Route : GeometryData, IRoute
    {
        public void Flip()
        {
            Points.Reverse();
            Geometry = GeometryFactory.ReversePoints(Points);
        }
    }
}