using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;

namespace BikeTouringGISLibrary
{
    public class BikeTouringGISGraphic : Graphic
    {
        public BikeTouringGISGraphic(Geometry geometry, GraphicType type) : base(geometry)
        {
            Type = type;
        }

        public GraphicType Type { get; private set; }

        public Graphic AddSymbol(Symbol symbol)
        {
            Symbol = symbol;
            return this;
        }
    }
}