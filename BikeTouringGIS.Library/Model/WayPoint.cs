using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;

namespace BikeTouringGISLibrary.Model
{
    public class WayPoint : GeometryCreatable
    {
        public string Name { get; internal set; }
        public Graphic Location
        {
            get
            {
                return CreatePointGraphic(Points[0], Name);
            }
        }

        internal override void CreateGeometry()
        {
        }
    }
}