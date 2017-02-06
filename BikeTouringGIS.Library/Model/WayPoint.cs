using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using BikeTouringGISLibrary.Enumerations;

namespace BikeTouringGISLibrary.Model
{
    public class WayPoint : GeometryCreatable
    {
        public string Name { get; internal set; }

        internal override void CreateGeometry()
        {
            Geometry = new MapPoint((double)Points[0].lon, (double)Points[0].lat, new SpatialReference(4326));
            Extent = Geometry.Extent;
        }

        public BikeTouringGISGraphic ToGraphic()
        {
            return CreateBikeTouringGISGraphic(Name, GraphicType.PointOfInterest);
        }
    }
}