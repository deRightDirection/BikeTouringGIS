using Esri.ArcGISRuntime.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace BikeTouringGISLibrary.Model
{
    public class GpxInformation
    {
        public GpxInformation()
        {
            Routes = new List<Route>();
            Tracks = new List<Track>();
            WayPoints = new List<WayPoint>();
        }
        public List<Route> Routes { get; set; }
        public List<Track> Tracks { get; set; }
        public List<WayPoint> WayPoints { get; set; }

        public Envelope WayPointsExtent
        {
            get
            {
                Envelope initialExtent = null;
                foreach (var wayPoint in WayPoints)
                {
                    var graphicExtent = wayPoint.Extent;
                    initialExtent = initialExtent == null ? initialExtent = graphicExtent : initialExtent = initialExtent.Union(graphicExtent);
                }
                return initialExtent;
            }
        }
    }
}