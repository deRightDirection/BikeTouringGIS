using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Geometry;
using GPX;
using System.Collections.Generic;

namespace BikeTouringGISLibrary.Model
{
    public class Route : GeometryCreatable, IRoute
    {
        public Route()
        {
        }

        public Route(List<wptType> points)
        {
            Points = points;
        }

        public BikeTouringGISGraphic EndLocation
        {
            get
            {
                return CreateBikeTouringGISPointGraphic(Points[Points.Count - 1], "end", GraphicType.GPXRouteEndLocation);
            }
        }

        public string Name { get; internal set; }

        public BikeTouringGISGraphic RouteGeometry
        {
            get
            {
                return CreateBikeTouringGISGraphic(Name, GraphicType.GPXRoute);
            }
        }

        public BikeTouringGISGraphic StartLocation
        {
            get
            {
                return CreateBikeTouringGISPointGraphic(Points[0], "start", GraphicType.GPXRouteStartLocation);
            }
        }

        public void Flip()
        {
            Points.Reverse();
            CreateGeometry();
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