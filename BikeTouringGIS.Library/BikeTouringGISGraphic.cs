using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;

namespace BikeTouringGISLibrary
{
    public class BikeTouringGISGraphic : Graphic
    {
        public BikeTouringGISGraphic(Geometry geometry) : base(geometry)
        {
        }

        public Graphic AddSymbol(Symbol symbol)
        {
            Symbol = symbol;
            return this;
        }
    }
}
