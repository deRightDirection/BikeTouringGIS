using BikeTouringGIS.Controls;
using BikeTouringGISLibrary;
using BikeTouringGISLibrary.Model;
using Esri.ArcGISRuntime.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGIS.Services
{
    public class LayerFactory
    {
        private Envelope _wayPointsExtent;

        public LayerFactory(Envelope wayPointsExtent)
        {
            _wayPointsExtent = wayPointsExtent;
        }

        internal List<BikeTouringGISLayer> CreateRoutes(string fileName, List<Route> routes)
        {
            var result = new List<BikeTouringGISLayer>();
            foreach (Route route in routes)
            {
                var layer = new BikeTouringGISLayer(fileName, route);
                layer.SetExtentToFitWithWaypoints(_wayPointsExtent);
                result.Add(layer);
            }
            return result;
        }

        internal List<BikeTouringGISLayer> CreateTracks(string fileName, List<Track> tracks)
        {
            var result = new List<BikeTouringGISLayer>();
            foreach (Track track in tracks)
            {
                var layer = new BikeTouringGISLayer(fileName, track);
                layer.SetExtentToFitWithWaypoints(_wayPointsExtent);
                result.Add(layer);
            }
            return result;
        }
    }
}