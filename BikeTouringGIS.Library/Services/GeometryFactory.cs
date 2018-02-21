using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Model;
using Esri.ArcGISRuntime.Geometry;
using GPX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISLibrary.Services
{
    public class GeometryFactory
    {
        public static void CreateGeometriesFromGpxFile(GpxInformation gpxFileInformation)
        {

            throw new NotImplementedException();
        }


        protected BikeTouringGISGraphic CreateBikeTouringGISGraphic(string nameAttribute, GraphicType typeOfGraphic)
        {
            var g = new BikeTouringGISGraphic(Geometry, typeOfGraphic);
            g.Attributes["name"] = nameAttribute;
            return g;
        }

        protected BikeTouringGISGraphic CreateBikeTouringGISPointGraphic(wptType location, string nameAttribute, GraphicType typeOfPoint)
        {
            var mappoint = new MapPoint((double)location.lon, (double)location.lat, new SpatialReference(4326));
            var g = new BikeTouringGISGraphic(mappoint, typeOfPoint);
            g.ZIndex = 1;
            g.Attributes["name"] = nameAttribute;
            return g;
        }

        internal static BikeTouringGISGraphic ReversePoints(List<wptType> points)
        {
            throw new NotImplementedException();
        }

        public trksegType[] Segments { get; internal set; }

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
