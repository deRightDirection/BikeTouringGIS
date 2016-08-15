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
        public List<IRoute> AllRoutes
        {
            get
            {
                var routes = Routes;
                routes.AddRange(Tracks.Where(t => t.IsConvertedToRoute));
                return routes.ToList<IRoute>();
            }
        }
        public List<Route> Routes { get; private set;}
        public List<Track> Tracks { get; private set; }
        public List<WayPoint> WayPoints { get; private set; }

        public void CreateGeometries()
        {
            foreach(var route in AllRoutes.Cast<Route>())
            {
                route.CreateGeometry();
            }
            WayPoints.ForEach(wp => wp.CreateGeometry());
        }

        public GpxInformation()
        {
            Routes = new List<Route>();
            Tracks = new List<Track>();
            WayPoints = new List<WayPoint>();
        }

        public Envelope GetExtent()
        {
            var items = AllRoutes.Cast<Route>().ToList();
            if(items.Count > 0)
            {
                var initialExtent = items[0].Extent;
                for(int i = 1; i < items.Count; i++)
                {
                    initialExtent = initialExtent.Union(items[i].Extent);
                }
                if(WayPoints.Count > 0)
                {
                    WayPoints.ForEach(wp => initialExtent = initialExtent.Union(wp.Extent));
                }
                return initialExtent;
            }
            return null;
        }
    }
}