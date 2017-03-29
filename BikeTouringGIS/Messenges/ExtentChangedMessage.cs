using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;

namespace BikeTouringGIS.Messenges
{
    class ExtentChangedMessage
    {
        public Envelope Extent { get; internal set; }
    }
}
