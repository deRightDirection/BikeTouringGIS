using GPX;
using System.Collections.Generic;

namespace BikeTouringGISLibrary
{
    public interface IRoute
    {
        BikeTouringGISGraphic EndLocation { get; }
        string Name { get; }
        List<wptType> Points { get; }
        BikeTouringGISGraphic RouteGeometry { get; }
        BikeTouringGISGraphic StartLocation { get; }

        void Flip();
    }
}