using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.GPX;
using System.Collections.Generic;

namespace BikeTouringGISLibrary
{
    public interface IPath
    {
        BikeTouringGISGraphic EndLocation { get; }
        string Name { get; }
        List<wptType> Points { get; }
        BikeTouringGISGraphic Geometry { get; }
        BikeTouringGISGraphic StartLocation { get; }
        PathType Type { get; set; }
    }
}