using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;
using BikeTouringGISLibrary.Enumerations;

namespace BikeTouringGISLibrary
{
    public class BikeTouringGISGraphic : Graphic
    {
        public GraphicType Type { get; private set;}
        public BikeTouringGISGraphic(Geometry geometry, GraphicType type) : base(geometry)
        {
            Type = type;
        }

        public Graphic AddSymbol(Symbol symbol)
        {
            Symbol = symbol;
            return this;
        }
    }
}