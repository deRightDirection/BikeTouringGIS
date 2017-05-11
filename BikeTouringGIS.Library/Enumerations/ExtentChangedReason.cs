using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISLibrary.Enumerations
{
    public enum ExtentChangedReason
    {
        Unknown,
        CenterMap,
        CenterLayer,
        ZoomIn,
        ZoomOut,
        Pan,
        StatusBarZoomInOrZoomOut
    }
}