using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPX;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;

namespace BikeTouringGISLibrary.Model
{
    public class Route : GeometryCreatable, IRoute
    {
        public string Name { get; internal set; }
        public BikeTouringGISGraphic StartLocation 
        {
            get
            {
                return CreatePointGraphic(Points[0], "start");
            }
        }
        public BikeTouringGISGraphic EndLocation
        {
            get
            {
                return CreatePointGraphic(Points[Points.Count - 1], "end");
            }
        }

        public BikeTouringGISGraphic RouteGeometry
        {
            get
            {
                return CreateLineGraphic(Name);
            }
        }

        internal override void CreateGeometry()
        {
            var builder = new PolylineBuilder(new SpatialReference(4326));
            foreach (var wayPoint in Points)
            {
                builder.AddPoint(new MapPoint((double)wayPoint.lon, (double)wayPoint.lat));
            }
            Geometry = builder.ToGeometry();
            Extent = Geometry.Extent;
        }
    }
}