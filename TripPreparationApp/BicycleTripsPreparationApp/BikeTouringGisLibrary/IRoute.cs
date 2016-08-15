using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using GPX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISLibrary
{
    public interface IRoute
    {
        List<wptType> Points { get; }
        string Name { get; }
        BikeTouringGISGraphic RouteGeometry { get; }
        BikeTouringGISGraphic StartLocation { get; }
        BikeTouringGISGraphic EndLocation { get; }
    }
}