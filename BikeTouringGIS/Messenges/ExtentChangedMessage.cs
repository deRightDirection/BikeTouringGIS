using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;
using BikeTouringGISLibrary.Enumerations;

namespace BikeTouringGIS.Messenges
{
    class ExtentChangedMessage
    {
        public ExtentChangedMessage(ExtentChangedReason reasonToChangeExtent)
        {
            ReasonToChangeExtent = reasonToChangeExtent;
        }
        public Envelope Extent { get; internal set; }
        public ExtentChangedReason ReasonToChangeExtent { get; private set; }
    }
}