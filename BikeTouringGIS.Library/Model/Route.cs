using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPX;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using BikeTouringGISLibrary.Enumerations;

namespace BikeTouringGISLibrary.Model
{
    public class Route : GeometryCreatable, IRoute
    {
        public string Name { get; internal set; }
        public BikeTouringGISGraphic StartLocation 
        {
            get
            {
                return CreateBikeTouringGISPointGraphic(Points[0], "start", GraphicType.GPXRouteStartLocation);
            }
        }
        public BikeTouringGISGraphic EndLocation
        {
            get
            {
                return CreateBikeTouringGISPointGraphic(Points[Points.Count - 1], "end", GraphicType.GPXRouteEndLocation);
            }
        }

        public BikeTouringGISGraphic RouteGeometry
        {
            get
            {
                return CreateBikeTouringGISGraphic(Name, GraphicType.GPXRoute);
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