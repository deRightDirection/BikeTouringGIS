using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeTouringGISLibrary.Model;
using GPX;

namespace BikeTouringGISLibrary.Services
{
    class GeometryOperator
    {
        internal static void ReverseRoute(Route route)
        {
            route.Points.Reverse();
            var geometryFactory = new GeometryFactory();
            geometryFactory.CreateRouteGeometry(route);
        }
    }
}
