using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using GPX;

namespace BicycleTripsPreparationApp
{
    class TrackReader
    {
        private string _gpxFileName;
        public TrackReader(string pathToGpxFile)
        {
            _gpxFileName = pathToGpxFile;
        }

        internal void ReadGPXTrack()
        {
            var file = new GPXFile(_gpxFileName);
            var route = file.GetRoutes()[0];
            WayPoints= route.RouteWayPoints;
        }

        public List<wptType> WayPoints { get; private set; }
    }
}
