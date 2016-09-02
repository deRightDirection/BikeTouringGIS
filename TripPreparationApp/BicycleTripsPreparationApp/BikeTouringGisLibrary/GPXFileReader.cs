using BikeTouringGISLibrary.Model;
using GPX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISLibrary
{
    public class GpxFileReader
    {
        private GpxInformation _gpxData;

        public GpxInformation LoadFile(string fileName)
        {
            _gpxData = new GpxInformation();
            var gpx = new GPXFile(fileName);
            LoadRoutes(gpx);
//            LoadWayPoints(gpx);
            LoadTracks(gpx);
            //            var waypoints = gpx.GetWaypoints();
            return _gpxData;
        }

        private void LoadTracks(GPXFile gpx)
        {
            var trackIndex = 1;
            foreach (var track in gpx.GetTracks())
            {
                var gpxTrack = new Track();
                if (string.IsNullOrEmpty(track.name))
                {
                    track.name = $"t{trackIndex.ToString()}";
                    trackIndex++;
                }
                gpxTrack.Name = track.name;
                gpxTrack.Segments = track.trkseg;
                _gpxData.Tracks.Add(gpxTrack);
            }
        }


        private void LoadWayPoints(GPXFile gpx)
        {
            throw new NotImplementedException();
        }

        private void LoadRoutes(GPXFile gpx)
        {
            var routeIndex = 1;
            foreach (var route in gpx.GetRoutes())
            {
                var gpxRoute = new Route();
                if (string.IsNullOrEmpty(route.name))
                {
                    route.name = string.Format("r{0}",routeIndex.ToString());
                    routeIndex++;
                }
                gpxRoute.Name = route.name;
                gpxRoute.Points = route.RouteWayPoints;
                _gpxData.Routes.Add(gpxRoute);
            }
        }
    }
}
