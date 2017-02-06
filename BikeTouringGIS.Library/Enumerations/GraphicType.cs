using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISLibrary.Enumerations
{
    public enum GraphicType
    {
        Unknown,
        GPXRouteStartLocation,
        GPXRouteEndLocation,
        GPXRoute,
        SplitPoint,
        SplitRoute,
        SplitPointLabel,
        PointOfInterest
    }
}
