using BikeTouringGIS.Controls;
using BikeTouringGISLibrary;
using BikeTouringGISLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGIS.Services
{
    public class LayerFactory
    {
        internal static List<BikeTouringGISLayer> CreateRoutes(List<Route> routes)
        {
            var result = new List<BikeTouringGISLayer>();   
            foreach (IRoute route in routes)
            {
                var layer = new BikeTouringGISLayer(path, route);
                layer.SetExtentToFitWithWaypoints(gpxFileInformation.WayPointsExtent);
                result.Add(layer);
            }
            return result;
        }

        internal static List<BikeTouringGISLayer> CreateTracks(List<Track> tracks)
        {
            var result = new List<BikeTouringGISLayer>();
            foreach (Track track in tracks)
            {
                result.Add(new BikeTouringGISLayer(path, track));
            }
            return result;
        }
    }
}
