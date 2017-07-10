using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Geometry;

namespace BikeTouringGIS.Messenges
{
    internal class ExtentChangedMessage
    {
        public ExtentChangedMessage(ExtentChangedReason reasonToChangeExtent)
        {
            ReasonToChangeExtent = reasonToChangeExtent;
        }

        public Envelope Extent { get; internal set; }
        public ExtentChangedReason ReasonToChangeExtent { get; private set; }
        public double ZoomFactor { get; internal set; }
    }
}