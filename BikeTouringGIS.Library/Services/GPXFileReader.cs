using BikeTouringGISLibrary.Model;
using GPX;
using System.Collections.Generic;
using System.IO;

namespace BikeTouringGISLibrary.Services
{
    public class GpxFileReader
    {
        private string _fileName;
        private GpxInformation _gpxData;

        public GpxInformation LoadFile(string fileName)
        {
            _gpxData = new GpxInformation();
            _fileName = fileName;
            var gpx = new GPXFile(_fileName);
            LoadRoutes(gpx);
            LoadWayPoints(gpx);
            LoadTracks(gpx);
            _gpxData.FileName = Path.GetFileName(fileName);
            return _gpxData;
        }

        private void LoadRoutes(GPXFile gpx)
        {
            var routeIndex = 1;
            foreach (var route in gpx.GetRoutes())
            {
                var gpxRoute = new Route();
                if (string.IsNullOrEmpty(route.name))
                {
                    route.name = string.Format("r{0}", routeIndex.ToString());
                    routeIndex++;
                }
                gpxRoute.Name = route.name;
                gpxRoute.Points = route.RouteWayPoints;
                _gpxData.Routes.Add(gpxRoute);
            }
        }

        private void LoadTracks(GPXFile gpx)
        {
            var trackIndex = 1;
            foreach (var track in gpx.GetTracks())
            {
                var gpxTrack = new Track();
                if (string.IsNullOrEmpty(track.name) && string.IsNullOrEmpty(gpx.Name))
                {
                    track.name = $"t{trackIndex.ToString()}";
                    trackIndex++;
                }
                gpxTrack.Name = string.IsNullOrEmpty(track.name) ? gpx.Name : track.name;
                gpxTrack.Segments = track.trkseg;
                gpxTrack.FileName = _fileName;
                _gpxData.Tracks.Add(gpxTrack);
            }
        }

        private void LoadWayPoints(GPXFile gpx)
        {
            foreach (var waypoint in gpx.GetWaypoints())
            {
                var wPoint = new WayPoint();
                wPoint.Name = waypoint.name;
                wPoint.Source = _fileName;
                wPoint.Points = new List<wptType>() { waypoint };
                _gpxData.WayPoints.Add(wPoint);
            }
        }
    }
}