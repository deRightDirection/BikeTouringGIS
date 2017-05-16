using Esri.ArcGISRuntime.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISLibrary.Model
{
    public class GpxInformation
    {
        public IEnumerable<Route> AllRoutes { get; private set; }
        public List<Route> Routes { get; private set;}
        public List<Track> Tracks { get; private set; }
        public List<WayPoint> WayPoints { get; private set; }
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

        public void CreateGeometries()
        {
            WayPoints.ForEach(wp => wp.CreateGeometry());
            var routes = Routes;
            routes.AddRange(Tracks.Where(t => t.IsConvertedToRoute));
            AllRoutes = routes.Cast<Route>();
            foreach (var route in AllRoutes)
            {
                route.CreateGeometry();
            }
        }

        public GpxInformation()
        {
            Routes = new List<Route>();
            Tracks = new List<Track>();
            WayPoints = new List<WayPoint>();
        }
    }
}