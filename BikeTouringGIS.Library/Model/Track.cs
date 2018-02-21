using Esri.ArcGISRuntime.Geometry;
using GPX;
using System.Collections.Generic;

namespace BikeTouringGISLibrary.Model
{
    public class Track : GeometryData
    {
        public bool IsConvertedToRoute { get; set; }
    }
}