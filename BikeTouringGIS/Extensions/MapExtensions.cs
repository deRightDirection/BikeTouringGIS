using BikeTouringGIS.Controls;
using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGIS.Extensions
{
    public static class MapExtensions
    {
        public static IEnumerable<BikeTouringGISLayer> GetBikeTouringGISLayers(this Map map)
        {
            var items = map.Layers.Where(x => x is BikeTouringGISLayer).Cast<BikeTouringGISLayer>();
            return items.Where(x => x.Type == LayerType.PointsOfInterest || x.Type == LayerType.GPXRoute).OrderBy(x => x.Type);
        }
    }
}